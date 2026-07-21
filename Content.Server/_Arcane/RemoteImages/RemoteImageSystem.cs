using System.Buffers.Binary;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Content.Shared._Arcane.RemoteImages;
using Content.Shared.GameTicking;
using Robust.Shared.Network;
using Robust.Shared.Network.Transfer;
using Robust.Shared.Player;

namespace Content.Server._Arcane.RemoteImages;

public sealed partial class RemoteImageSystem : EntitySystem
{
    private const int ImageBufferCapacity = 10;
    private const int DownloadTimeoutSeconds = 30;

    private static readonly object TransferRegistrationLock = new();
    private static readonly System.Runtime.CompilerServices.ConditionalWeakTable<ITransferManager, object>
        RegisteredTransferManagers = new();

    [Dependency] private IHttpClientHolder _http = default!;
    [Dependency] private ITransferManager _transferManager = default!;

    private readonly Lock _imageBufferLock = new();
    private readonly Dictionary<string, byte[]> _imageBuffer = new(StringComparer.Ordinal);
    private readonly Queue<string> _imageBufferOrder = new();
    private readonly Dictionary<string, Task<byte[]>> _pendingDownloads = new(StringComparer.Ordinal);

    public override void Initialize()
    {
        RegisterTransferMessage();
        SubscribeNetworkEvent<RequestRemoteImageEvent>(OnImageRequested);
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawned);
    }

    private void RegisterTransferMessage()
    {
        lock (TransferRegistrationLock)
        {
            if (RegisteredTransferManagers.TryGetValue(_transferManager, out _))
                return;

            _transferManager.RegisterTransferMessage(RemoteImageConstants.TransferKey);
            RegisteredTransferManagers.Add(_transferManager, new object());
        }
    }

    private async void OnImageRequested(RequestRemoteImageEvent ev, EntitySessionEventArgs args)
    {
        var filter = Filter.SinglePlayer(args.SenderSession);

        try
        {
            if (!RemoteImageConstants.TryNormalizeImgboxPngUrl(ev.ImageUrl, out var imageUrl)
                || string.IsNullOrEmpty(imageUrl))
            {
                throw new InvalidOperationException("Разрешены только прямые HTTPS-ссылки на PNG с imgbox.");
            }

            var image = await GetImage(imageUrl);
            await SendImage(args.SenderSession.Channel, ev.RequestId, image);
        }
        catch (Exception exception)
        {
            Log.Warning($"Failed to download character image for {args.SenderSession.Name}: {exception.Message}");
            RaiseNetworkEvent(
                new RemoteImageErrorEvent(ev.RequestId, $"Не удалось загрузить изображение: {exception.Message}"),
                filter,
                false);
        }
    }

    private void OnPlayerSpawned(PlayerSpawnCompleteEvent ev)
    {
        if (!RemoteImageConstants.TryNormalizeImgboxPngUrl(ev.Profile.CharacterImageUrl, out var imageUrl)
            || string.IsNullOrEmpty(imageUrl))
        {
            return;
        }

        _ = PrefetchImage(imageUrl);
    }

    private async Task PrefetchImage(string imageUrl)
    {
        try
        {
            await GetImage(imageUrl);
        }
        catch (Exception exception)
        {
            Log.Debug($"Failed to prefetch character image: {exception.Message}");
        }
    }

    private async Task SendImage(INetChannel channel, int requestId, byte[] image)
    {
        await using var transfer = _transferManager.StartTransfer(channel, RemoteImageConstants.TransferKey);
        var header = new byte[8];
        BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(0, 4), requestId);
        BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(4, 4), image.Length);

        await transfer.WriteAsync(header);
        await transfer.WriteAsync(image);
    }

    private Task<byte[]> GetImage(string imageUrl)
    {
        lock (_imageBufferLock)
        {
            if (_imageBuffer.TryGetValue(imageUrl, out var image))
                return Task.FromResult(image);

            if (_pendingDownloads.TryGetValue(imageUrl, out var pending))
                return pending;

            var completion = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
            _pendingDownloads[imageUrl] = completion.Task;
            _ = DownloadAndBufferImage(imageUrl, completion);
            return completion.Task;
        }
    }

    private async Task DownloadAndBufferImage(string imageUrl, TaskCompletionSource<byte[]> completion)
    {
        try
        {
            var image = await DownloadImage(imageUrl);

            lock (_imageBufferLock)
            {
                if (_imageBuffer.TryGetValue(imageUrl, out var bufferedImage))
                {
                    completion.TrySetResult(bufferedImage);
                    return;
                }

                while (_imageBufferOrder.Count >= ImageBufferCapacity)
                {
                    var oldestUrl = _imageBufferOrder.Dequeue();
                    _imageBuffer.Remove(oldestUrl);
                }

                _imageBuffer[imageUrl] = image;
                _imageBufferOrder.Enqueue(imageUrl);
            }

            completion.TrySetResult(image);
        }
        catch (Exception exception)
        {
            completion.TrySetException(exception);
        }
        finally
        {
            lock (_imageBufferLock)
            {
                _pendingDownloads.Remove(imageUrl);
            }
        }
    }

    private async Task<byte[]> DownloadImage(string imageUrl)
    {
        if (TryCreateThumbnailUrl(imageUrl, out var thumbnailUrl))
        {
            try
            {
                return await DownloadImage(thumbnailUrl, true);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                Log.Debug($"Failed to download imgbox thumbnail, falling back to the original: {exception.Message}");
            }
        }

        return await DownloadImage(imageUrl, false);
    }

    private async Task<byte[]> DownloadImage(string imageUrl, bool thumbnail)
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(DownloadTimeoutSeconds));
        using var response = await _http.Client.GetAsync(
            imageUrl,
            HttpCompletionOption.ResponseHeadersRead,
            cancellation.Token);

        response.EnsureSuccessStatusCode();

        var finalUri = response.RequestMessage?.RequestUri;
        if (finalUri == null || !Uri.Compare(
                finalUri,
                new Uri(imageUrl),
                UriComponents.HttpRequestUrl,
                UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase).Equals(0))
        {
            throw new InvalidOperationException("imgbox перенаправил запрос на запрещённый адрес.");
        }

        if (!string.Equals(response.Content.Headers.ContentType?.MediaType, "image/png", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Ссылка вернула не PNG.");

        if (response.Content.Headers.ContentLength > RemoteImageConstants.MaxImageBytes)
            throw new InvalidOperationException("Изображение превышает лимит 5 МБ.");

        await using var source = await response.Content.ReadAsStreamAsync(cancellation.Token);
        using var destination = new MemoryStream();
        var buffer = new byte[81920];

        while (true)
        {
            var read = await source.ReadAsync(buffer, cancellation.Token);
            if (read == 0)
                break;

            if (destination.Length + read > RemoteImageConstants.MaxImageBytes)
                throw new InvalidOperationException("Изображение превышает лимит 5 МБ.");

            destination.Write(buffer, 0, read);
        }

        var image = destination.ToArray();
        if (thumbnail)
            ValidateThumbnail(image);
        else
            ValidatePng(image);

        return image;
    }

    private static bool TryCreateThumbnailUrl(string imageUrl, out string thumbnailUrl)
    {
        thumbnailUrl = string.Empty;
        var uri = new Uri(imageUrl);
        if (!uri.AbsolutePath.EndsWith("_o.png", StringComparison.OrdinalIgnoreCase)
            || !uri.Host.StartsWith("images", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var builder = new UriBuilder(uri)
        {
            Host = $"thumbs{uri.Host["images".Length..]}",
            Path = $"{uri.AbsolutePath[..^"_o.png".Length]}_t.png",
        };
        thumbnailUrl = builder.Uri.AbsoluteUri;
        return true;
    }

    private static void ValidateThumbnail(byte[] image)
    {
        // Imgbox serves its PNG thumbnails as JPEG data despite the .png URL and content type.
        if (image.Length < 4
            || image[0] != 0xFF
            || image[1] != 0xD8
            || image[^2] != 0xFF
            || image[^1] != 0xD9)
        {
            throw new InvalidOperationException("Полученная миниатюра имеет неизвестный формат.");
        }
    }

    private static void ValidatePng(byte[] image)
    {
        if (image.Length < 24
            || image[0] != 0x89
            || image[1] != 0x50
            || image[2] != 0x4E
            || image[3] != 0x47
            || image[12] != 0x49
            || image[13] != 0x48
            || image[14] != 0x44
            || image[15] != 0x52)
        {
            throw new InvalidOperationException("Полученные данные не являются PNG.");
        }

        var width = BinaryPrimitives.ReadUInt32BigEndian(image.AsSpan(16, 4));
        var height = BinaryPrimitives.ReadUInt32BigEndian(image.AsSpan(20, 4));
        if (width == 0
            || height == 0
            || width > RemoteImageConstants.MaxImageDimension
            || height > RemoteImageConstants.MaxImageDimension
            || (long) width * height > RemoteImageConstants.MaxImagePixels)
        {
            throw new InvalidOperationException("Разрешение изображения слишком велико.");
        }
    }
}
