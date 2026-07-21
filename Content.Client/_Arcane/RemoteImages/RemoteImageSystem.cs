using System.Buffers.Binary;
using System.IO;
using System.Threading.Tasks;
using Content.Shared._Arcane.RemoteImages;
using Robust.Client.Graphics;
using Robust.Shared.Network.Transfer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Client._Arcane.RemoteImages;

public sealed partial class RemoteImageSystem : EntitySystem
{
    private const int ImageBufferCapacity = 10;

    private static readonly object TransferRegistrationLock = new();
    private static readonly Dictionary<ITransferManager, TransferRegistration> TransferRegistrations = new();

    [Dependency] private IClyde _clyde = default!;
    [Dependency] private ITransferManager _transferManager = default!;

    private readonly Dictionary<int, RequestState> _requests = new();
    private readonly Dictionary<string, byte[]> _imageBuffer = new(StringComparer.Ordinal);
    private readonly Queue<string> _imageBufferOrder = new();
    private int _nextRequestId;

    public override void Initialize()
    {
        RegisterTransferMessage();
        SubscribeNetworkEvent<RemoteImageErrorEvent>(OnError);
    }

    private void RegisterTransferMessage()
    {
        lock (TransferRegistrationLock)
        {
            if (!TransferRegistrations.TryGetValue(_transferManager, out var registration))
            {
                registration = new TransferRegistration();
                _transferManager.RegisterTransferMessage(RemoteImageConstants.TransferKey, registration.Receive);
                TransferRegistrations.Add(_transferManager, registration);
            }

            registration.Handler = OnImageTransfer;
        }
    }

    public int RequestImage(
        string imageUrl,
        Action<OwnedTexture, int> onSuccess,
        Action<string> onError,
        Action<int, int>? onProgress = null)
    {
        if (!RemoteImageConstants.TryNormalizeImgboxPngUrl(imageUrl, out var normalized)
            || string.IsNullOrEmpty(normalized))
        {
            onError(Loc.GetString("arcane-character-image-invalid-url"));
            return 0;
        }

        if (_imageBuffer.TryGetValue(normalized, out var bufferedImage))
        {
            LoadTexture(bufferedImage, 0, onSuccess, onError);
            return 0;
        }

        var requestId = ++_nextRequestId;
        _requests[requestId] = new RequestState(normalized, onSuccess, onError, onProgress);
        RaiseNetworkEvent(new RequestRemoteImageEvent(requestId, normalized));
        return requestId;
    }

    public void CancelRequest(int requestId)
    {
        if (requestId != 0)
            _requests.Remove(requestId);
    }

    private async void OnImageTransfer(TransferReceivedEvent transfer)
    {
        await using var stream = transfer.DataStream;

        var header = new byte[8];
        try
        {
            await stream.ReadExactlyAsync(header);
            var requestId = BinaryPrimitives.ReadInt32LittleEndian(header.AsSpan(0, 4));
            var totalBytes = BinaryPrimitives.ReadInt32LittleEndian(header.AsSpan(4, 4));

            if (totalBytes <= 0 || totalBytes > RemoteImageConstants.MaxImageBytes)
            {
                Fail(requestId, Loc.GetString("arcane-character-image-invalid-size"));
                return;
            }

            if (!_requests.TryGetValue(requestId, out var request))
            {
                await DrainTransfer(stream);
                return;
            }

            var image = new byte[totalBytes];
            var receivedBytes = 0;
            request.OnProgress?.Invoke(0, totalBytes);

            while (receivedBytes < totalBytes)
            {
                var read = await stream.ReadAsync(image.AsMemory(receivedBytes, totalBytes - receivedBytes));
                if (read == 0)
                {
                    Fail(requestId, Loc.GetString("arcane-character-image-invalid-transfer"));
                    return;
                }

                receivedBytes += read;
                request.OnProgress?.Invoke(receivedBytes, totalBytes);
            }

            _requests.Remove(requestId);
            BufferImage(request.ImageUrl, image);
            LoadTexture(image, requestId, request.OnSuccess, request.OnError);
        }
        catch (Exception exception)
        {
            var requestId = BinaryPrimitives.ReadInt32LittleEndian(header.AsSpan(0, 4));
            Fail(requestId, Loc.GetString("arcane-character-image-invalid-transfer"));
            Log.Warning($"Failed to receive character image: {exception}");
        }
    }

    private static async Task DrainTransfer(Stream stream)
    {
        var buffer = new byte[81920];
        while (await stream.ReadAsync(buffer) != 0)
        {
        }
    }

    private void BufferImage(string imageUrl, byte[] image)
    {
        if (_imageBuffer.ContainsKey(imageUrl))
            return;

        while (_imageBufferOrder.Count >= ImageBufferCapacity)
        {
            var oldestUrl = _imageBufferOrder.Dequeue();
            _imageBuffer.Remove(oldestUrl);
        }

        _imageBuffer[imageUrl] = image;
        _imageBufferOrder.Enqueue(imageUrl);
    }

    private void LoadTexture(
        byte[] image,
        int requestId,
        Action<OwnedTexture, int> onSuccess,
        Action<string> onError)
    {
        try
        {
            using var stream = new MemoryStream(image, writable: false);
            using var decodedImage = Image.Load<Rgba32>(stream);
            if (decodedImage.Width > RemoteImageConstants.MaxImageDimension
                || decodedImage.Height > RemoteImageConstants.MaxImageDimension
                || (long) decodedImage.Width * decodedImage.Height > RemoteImageConstants.MaxImagePixels)
            {
                throw new InvalidOperationException(Loc.GetString("arcane-character-image-invalid-size"));
            }

            var texture = _clyde.LoadTextureFromImage(decodedImage, $"Arcane character image {requestId}");
            onSuccess(texture, image.Length);
        }
        catch (Exception exception)
        {
            onError(Loc.GetString("arcane-character-image-decode-error", ("error", exception.Message)));
        }
    }

    private void OnError(RemoteImageErrorEvent ev)
    {
        Fail(ev.RequestId, ev.Message);
    }

    private void Fail(int requestId, string message)
    {
        if (!_requests.Remove(requestId, out var request))
            return;

        request.OnError(message);
    }

    private sealed class RequestState(
        string imageUrl,
        Action<OwnedTexture, int> onSuccess,
        Action<string> onError,
        Action<int, int>? onProgress)
    {
        public readonly string ImageUrl = imageUrl;
        public readonly Action<OwnedTexture, int> OnSuccess = onSuccess;
        public readonly Action<string> OnError = onError;
        public readonly Action<int, int>? OnProgress = onProgress;
    }

    private sealed class TransferRegistration
    {
        public Action<TransferReceivedEvent>? Handler;

        public void Receive(TransferReceivedEvent transfer)
        {
            Handler?.Invoke(transfer);
        }
    }
}
