using System.Threading;
using System.Threading.Tasks;
using Content.Server._Starlight.Language;
using Content.Server._Starlight.TextToSpeech;
using Content.Server.Chat.Systems;
using Content.Server.Radio.EntitySystems;
using Content.Shared._Arcane.CCVars;
using Content.Shared._Arcane.TTS;
using Content.Shared._Starlight.Language;
using Content.Shared._Starlight.Language.Components;
using Content.Shared._Starlight.TextToSpeech;
using Content.Shared.Chat;
using Content.Shared.GameTicking;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server._Arcane.TTS;

public sealed partial class ArcaneTTSSystem : EntitySystem
{
    [Dependency] private IConfigurationManager _cfg = default!;
    [Dependency] private IPrototypeManager _prototypeManager = default!;
    [Dependency] private ArcaneTTSManager _ttsManager = default!;
    [Dependency] private SharedTransformSystem _xforms = default!;
    [Dependency] private LanguageSystem _language = default!;

    private const int MaxMessageChars = 300;
    private bool _isEnabled;

    public override void Initialize()
    {
        _cfg.OnValueChanged(ACCVars.TTSEnabled, v => _isEnabled = v, true);

        SubscribeLocalEvent<EntitySpokeEvent>(OnEntitySpoke, after: [typeof(RadioSystem), typeof(HeadsetSystem)]);
        SubscribeLocalEvent<TransformSpeechEvent>(OnTransformSpeech);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(_ => _ttsManager.ResetCache());
        SubscribeLocalEvent<RadioSpokeEvent>(OnRadioSpoke);
        SubscribeLocalEvent<AnnouncementSpokeEvent>(OnAnnouncementSpoke);

        SubscribeNetworkEvent<RequestPreviewTTSEvent>(OnRequestPreviewTTS);
    }

    private void OnEntitySpoke(EntitySpokeEvent args)
    {
        if (!_isEnabled || args.Message.Text.Length > MaxMessageChars)
            return;

        if (args.Channel != null)
            return;

        if (args.Language.Speech.BlockSpeech)
            return;

        if (!TryComp<TextToSpeechComponent>(args.Source, out var tts))
            return;

        var voiceId = tts.VoicePrototypeId;
        var effect = tts.Effect;

        if (voiceId == null
            || !_prototypeManager.TryIndex<VoicePrototype>(voiceId, out var protoVoice)
            || protoVoice.ArcaneVoice is not { } speaker)
            return;

        if (args.IsWhisper)
        {
            HandleWhisper(args.Source, args.Message.Text, args.Language, speaker);
            return;
        }

        HandleSay(args.Source, args.Message.Text, args.Language, speaker, effect);
    }

    private void OnRadioSpoke(RadioSpokeEvent args)
    {
        var message = args.Message.Tts ?? args.Message.Text;
        if (!_isEnabled
            || args.SuppressTTS
            || message.Length > MaxMessageChars
            || !TryGetArcaneVoice(args.Source, out var speaker))
            return;

        HandleReceiveRadio(Filter.Entities(args.Receivers), message, speaker, "radio_headset", args.Language);
    }

    private void OnAnnouncementSpoke(AnnouncementSpokeEvent args)
    {
        if (!_isEnabled
            || args.Message.Text.Length > MaxMessageChars * 2
            || args.SpeakerUid is not { } speakerNetEntity
            || !TryGetArcaneVoice(GetEntity(speakerNetEntity), out var speaker))
            return;

        Robust.Shared.Timing.Timer.Spawn(TimeSpan.FromSeconds(3),
            () => HandleReceiveRadio(args.Receivers, args.Message.Tts ?? args.Message.Text, speaker, "announce"));
    }

    private async void HandleReceiveRadio(Filter filter, string message, string speaker, string effect, LanguagePrototype? language = null)
    {
        var soundData = await GenerateTTS(message, speaker, effect);
        if (soundData is null)
            return;

        foreach (var recipient in filter.Recipients)
        {
            var uid = recipient.AttachedEntity;
            if (uid == null)
                continue;

            if (language != null && !_language.CanUnderstand(uid.Value, language.ID))
                continue;

            RaiseNetworkEvent(new PlayTTSEvent(soundData, useRadioVolume: true), recipient);
        }
    }

    private bool TryGetArcaneVoice(EntityUid uid, out string speaker)
    {
        speaker = string.Empty;
        return TryComp<TextToSpeechComponent>(uid, out var component)
            && component.VoicePrototypeId is { } voiceId
            && _prototypeManager.TryIndex<VoicePrototype>(voiceId, out var voice)
            && voice.ArcaneVoice is { } arcaneVoice
            && (speaker = arcaneVoice) != string.Empty;
    }

    private async void HandleSay(EntityUid uid, string message, LanguagePrototype language, string speaker, string? effect)
    {
        var normal = await GenerateTTS(message, speaker, effect);
        if (normal is null)
            return;

        var nilter = Filter.Empty();
        var lilter = Filter.Empty();
        foreach (var session in Filter.Pvs(uid).Recipients)
        {
            if (!session.AttachedEntity.HasValue)
                continue;

            TryComp(session.AttachedEntity.Value, out LanguageSpeakerComponent? lang);

            if (_language.CanUnderstand(new(session.AttachedEntity.Value, lang), language.ID))
                nilter.AddPlayer(session);
            else
                lilter.AddPlayer(session);
        }

        RaiseNetworkEvent(new PlayTTSEvent(normal, GetNetEntity(uid)), nilter);
    }

    private async void HandleWhisper(EntityUid uid, string message, LanguagePrototype language, string speaker)
    {
        var normal = await GenerateTTS(message, speaker);
        if (normal is null)
            return;

        // TODO: Check obstacles
        var xformQuery = GetEntityQuery<TransformComponent>();
        var sourcePos = _xforms.GetWorldPosition(xformQuery.GetComponent(uid), xformQuery);
        var nilter = Filter.Empty();
        var lilter = Filter.Empty();
        foreach (var session in Filter.Pvs(uid).Recipients)
        {
            if (!session.AttachedEntity.HasValue)
                continue;

            var xform = xformQuery.GetComponent(session.AttachedEntity.Value);
            var distance = (sourcePos - _xforms.GetWorldPosition(xform, xformQuery)).Length();
            if (distance > ChatSystem.WhisperMuffledRange)
                continue;

            TryComp(session.AttachedEntity.Value, out LanguageSpeakerComponent? lang);

            if (_language.CanUnderstand(new(session.AttachedEntity.Value, lang), language.ID)
                && distance <= ChatSystem.WhisperClearRange)
                nilter.AddPlayer(session);
            else
                lilter.AddPlayer(session);
        }

        RaiseNetworkEvent(new PlayTTSEvent(normal, GetNetEntity(uid), true), nilter);
    }

    private readonly Dictionary<string, Task<byte[]?>> _ttsTasks = new();
    private readonly SemaphoreSlim _lock = new(1, 1);

    private async Task<byte[]?> GenerateTTS(string text, string speaker, string? effect = null)
    {
        var textSanitized = Sanitize(text);
        if (string.IsNullOrEmpty(textSanitized))
            return null;

        if (char.IsLetter(textSanitized[^1]))
            textSanitized += ".";

        var taskKey = $"{textSanitized}_{speaker}_{effect}";

        await _lock.WaitAsync();
        Task<byte[]?> task;
        try
        {
            if (_ttsTasks.TryGetValue(taskKey, out var existing))
                return await existing;

            task = _ttsManager.ConvertTextToSpeech(speaker, textSanitized, effect);
            _ttsTasks[taskKey] = task;
        }
        finally
        {
            _lock.Release();
        }

        try
        {
            return await task;
        }
        finally
        {
            await _lock.WaitAsync();
            try { _ttsTasks.Remove(taskKey); }
            finally { _lock.Release(); }
        }
    }
}
