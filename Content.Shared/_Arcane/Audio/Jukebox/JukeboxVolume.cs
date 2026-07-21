using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Audio.Jukebox;

public sealed partial class JukeboxComponent
{
    [DataField, AutoNetworkedField]
    public float Volume = SharedJukeboxSystem.DefaultSliderVolume;
}

public abstract partial class SharedJukeboxSystem
{
    public const float MinSliderVolume = 0f;
    public const float MaxSliderVolume = 100f;
    public const float DefaultSliderVolume = 50f;

    private const float MinAudioVolume = -30f;
    private const float MaxAudioVolume = 0f;

    public static float SliderToAudioVolume(float value)
    {
        var clamped = Math.Clamp(value, MinSliderVolume, MaxSliderVolume);
        return MinAudioVolume + clamped / MaxSliderVolume * (MaxAudioVolume - MinAudioVolume);
    }
}

[Serializable, NetSerializable]
public sealed class JukeboxSetVolumeMessage(float volume) : BoundUserInterfaceMessage
{
    public float Volume { get; } = volume;
}
