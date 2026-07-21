// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Arcane.CCVars;
using Content.Shared._Arcane.TTS;
using Content.Shared.Chat;
using Content.Shared.GameTicking;
using Robust.Client.Audio;
using Robust.Client.ResourceManagement;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.Utility;

namespace Content.Client._Arcane.TTS;

/// <summary>
/// Plays complete OGG payloads produced by the Arcane TTS provider.
/// </summary>
public sealed class ArcaneTTSSystem : EntitySystem
{
    [Dependency] private IConfigurationManager _cfg = default!;
    [Dependency] private IResourceManager _resources = default!;
    [Dependency] private AudioSystem _audio = default!;

    private const float WhisperVolumeReduction = 4f;

    private static ulong _sharedIndex;
    private readonly MemoryContentRoot _contentRoot = new();
    private ISawmill _sawmill = default!;
    private ResPath _prefix;
    private ulong _fileIndex;
    private float _volume;
    private float _radioVolume;
    private bool _enabled;

    public override void Initialize()
    {
        _prefix = ResPath.Root / $"ArcaneTTS{_sharedIndex++}";
        _sawmill = Logger.GetSawmill("arcane.tts");
        _resources.AddRoot(_prefix, _contentRoot);

        _cfg.OnValueChanged(ACCVars.TTSVolume, OnVolumeChanged, true);
        _cfg.OnValueChanged(ACCVars.TTSRadioVolume, OnRadioVolumeChanged, true);
        _cfg.OnValueChanged(ACCVars.UseTTS, OnEnabledChanged, true);

        SubscribeNetworkEvent<PlayTTSEvent>(OnPlayTTS);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(_ => _contentRoot.Clear());
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _cfg.UnsubValueChanged(ACCVars.TTSVolume, OnVolumeChanged);
        _cfg.UnsubValueChanged(ACCVars.TTSRadioVolume, OnRadioVolumeChanged);
        _cfg.UnsubValueChanged(ACCVars.UseTTS, OnEnabledChanged);
        _contentRoot.Clear();
        _contentRoot.Dispose();
    }

    public void RequestPreviewTts(string voiceId)
    {
        RaiseNetworkEvent(new RequestPreviewTTSEvent(voiceId));
    }

    private void OnVolumeChanged(float volume)
    {
        _volume = volume;
    }

    private void OnRadioVolumeChanged(float volume)
    {
        _radioVolume = volume;
    }

    private void OnEnabledChanged(bool enabled)
    {
        _enabled = enabled;
    }

    private void OnPlayTTS(PlayTTSEvent ev)
    {
        if (!_enabled)
            return;

        _sawmill.Verbose($"Play Arcane TTS audio ({ev.Data.Length} bytes) from {ev.SourceUid}");

        var filePath = new ResPath($"{_fileIndex++}.ogg");
        _contentRoot.AddOrUpdateFile(filePath, ev.Data);

        try
        {
            var audioResource = new AudioResource();
            audioResource.Load(IoCManager.Instance!, _prefix / filePath);

            var volume = ev.UseRadioVolume ? _radioVolume : _volume;
            var adjustedVolume = SharedAudioSystem.GainToVolume(volume);
            if (ev.IsWhisper)
                adjustedVolume -= SharedAudioSystem.GainToVolume(WhisperVolumeReduction);

            var audioParams = AudioParams.Default
                .WithVolume(adjustedVolume)
                .WithMaxDistance(ev.IsWhisper
                    ? SharedChatSystem.WhisperMuffledRange
                    : SharedChatSystem.VoiceRange);

            if (ev.SourceUid is { } sourceNetEntity)
            {
                var source = GetEntity(sourceNetEntity);
                if (source.IsValid())
                    _audio.PlayEntity(audioResource.AudioStream, source, null, audioParams);
            }
            else
            {
                _audio.PlayGlobal(audioResource.AudioStream, null, audioParams);
            }
        }
        finally
        {
            _contentRoot.RemoveFile(filePath);
        }
    }
}
