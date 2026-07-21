using Content.Shared.Audio.Jukebox;
using Robust.Shared.Audio.Components;

namespace Content.Client.Audio.Jukebox;

public sealed partial class JukeboxBoundUserInterface
{
    private void SetVolume(float volume)
    {
        if (EntMan.TryGetComponent(Owner, out JukeboxComponent? jukebox) &&
            EntMan.TryGetComponent(jukebox.AudioStream, out AudioComponent? audio))
        {
            audio.Volume = SharedJukeboxSystem.SliderToAudioVolume(volume);
        }

        SendMessage(new JukeboxSetVolumeMessage(volume));
    }
}
