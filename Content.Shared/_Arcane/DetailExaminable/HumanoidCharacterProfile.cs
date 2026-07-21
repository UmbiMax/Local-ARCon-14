using Content.Shared._Arcane.RemoteImages;

// ReSharper disable once CheckNamespace
namespace Content.Shared.Preferences;

public sealed partial class HumanoidCharacterProfile
{
    [DataField]
    public string CharacterImageUrl { get; set; } = string.Empty;

    public HumanoidCharacterProfile WithCharacterImageUrl(string imageUrl)
    {
        return new(this) { CharacterImageUrl = imageUrl };
    }

    public static string ValidateCharacterImageUrl(string? imageUrl)
    {
        return RemoteImageConstants.TryNormalizeImgboxPngUrl(imageUrl, out var normalized)
            ? normalized
            : string.Empty;
    }
}
