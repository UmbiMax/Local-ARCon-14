using Content.Shared.Audio.Jukebox;

namespace Content.Server.Audio.Jukebox;

public sealed partial class JukeboxSystem
{
    private void InitializeVolume()
    {
        SubscribeLocalEvent<JukeboxComponent, JukeboxSetVolumeMessage>(OnJukeboxSetVolume);
    }

    private void OnJukeboxSetVolume(EntityUid uid, JukeboxComponent component, JukeboxSetVolumeMessage args)
    {
        component.Volume = Math.Clamp(
            args.Volume,
            SharedJukeboxSystem.MinSliderVolume,
            SharedJukeboxSystem.MaxSliderVolume);

        Audio.SetVolume(component.AudioStream, SharedJukeboxSystem.SliderToAudioVolume(component.Volume));
        Dirty(uid, component);
    }
}
