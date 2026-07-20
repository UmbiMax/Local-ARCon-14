using Content.Shared.Humanoid;
using Robust.Shared.Prototypes;

namespace Content.Shared._Starlight.TextToSpeech;
/// <summary>
/// Prototype represent TTS voices
/// </summary>
[Prototype("voice")]
public sealed partial class VoicePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("voice")]
    public int Voice { get; private set; } = -1; // Arcane

    // Arcane-start
    /// <summary>
    /// Speaker identifier used by the Arcane TTS provider.
    /// A null value means that this voice belongs to another provider.
    /// </summary>
    [DataField]
    public string? ArcaneVoice { get; private set; }

    /// <summary>
    /// Whether the voice is available in the character editor.
    /// </summary>
    [DataField]
    public bool RoundStart { get; private set; } = true;
    // Arcane-end

    [DataField("name")]
    public string Name { get; private set; } = string.Empty;

    [DataField("sex", required: true)]
    public Sex Sex { get; private set; } = default!;

    [DataField("silicon")]
    public bool Silicon { get; private set; } = false;

    [DataField]
    public string? Copyright { get; private set; }

    [DataField]
    public string? License { get; private set; }
}
