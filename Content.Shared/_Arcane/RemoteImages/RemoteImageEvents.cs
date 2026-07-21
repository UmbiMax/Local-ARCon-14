using System.Text.RegularExpressions;
using Robust.Shared.Serialization;

namespace Content.Shared._Arcane.RemoteImages;

public static class RemoteImageConstants
{
    public const string TransferKey = "ArcaneRemoteImage";
    public const int MaxUrlLength = 512;
    public const int MaxImageBytes = 5 * 1024 * 1024;
    public const int MaxImageDimension = 4096;
    public const long MaxImagePixels = 16_777_216;

    private static readonly Regex ImgboxPngUrl = new(
        @"^https://images\d+\.imgbox\.com/[a-z0-9%_./~-]+\.png(?:\?[a-z0-9%&=_.~-]*)?$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    public static bool TryNormalizeImgboxPngUrl(string? value, out string normalized)
    {
        normalized = string.Empty;
        if (string.IsNullOrWhiteSpace(value))
            return true;

        value = value.Trim();
        if (value.Length > MaxUrlLength
            || !ImgboxPngUrl.IsMatch(value))
        {
            return false;
        }

        normalized = value;
        return true;
    }
}

[Serializable, NetSerializable]
public sealed class RequestRemoteImageEvent(int requestId, string imageUrl) : EntityEventArgs
{
    public int RequestId { get; } = requestId;
    public string ImageUrl { get; } = imageUrl;
}

[Serializable, NetSerializable]
public sealed class RemoteImageErrorEvent(int requestId, string message) : EntityEventArgs
{
    public int RequestId { get; } = requestId;
    public string Message { get; } = message;
}
