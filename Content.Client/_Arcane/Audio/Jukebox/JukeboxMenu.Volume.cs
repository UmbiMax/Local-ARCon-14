using Content.Shared.Audio.Jukebox;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Audio.Jukebox;

public sealed partial class JukeboxMenu
{
    public event Action<float>? SetVolume;

    private void InitializeVolumeControls()
    {
        VolumeSlider.MinValue = SharedJukeboxSystem.MinSliderVolume;
        VolumeSlider.MaxValue = SharedJukeboxSystem.MaxSliderVolume;
        VolumeSlider.OnReleased += OnVolumeSliderReleased;
    }

    private void OnVolumeSliderReleased(Slider slider)
    {
        SetVolume?.Invoke(slider.Value);
        _lockTimer = 0.5f;
    }

    public void SetVolumeSlider(float volume)
    {
        VolumeSlider.SetValueWithoutEvent(volume);
    }

    private void UpdateVolumeControls(bool locked)
    {
        VolumeSlider.Disabled = locked;
        VolumeNumberLabel.Text = $"{VolumeSlider.Value:0}%";
    }
}
