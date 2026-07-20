using Content.Shared._Arcane.TTS;
using Content.Shared._Starlight.TextToSpeech;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server._Arcane.TTS;

public sealed partial class ArcaneTTSSystem
{
    [Dependency] private IRobustRandom _robustRandom = default!;

    private readonly List<string> _sampleText = new()
    {
        "Бармен, сделай чего-то покрепче. Я после этой смены вообще людей не узнаю.",
        "Эй, не отключайся, слышишь? Медики уже бегут. Просто смотри на меня.",
        "Чёрт... Он же минуту назад шутил по рации. Заберите айди, я отнесу капитану.",
        "Тише, Тише... Не открывай дверь. Оно всё ещё там.",
        "Да положи ты пушку на пол! Я второй раз повторять не буду!",
        "Атмос, мостик в разгерметизации! Быстрее сюда!",
        "Так, без паники. Раненых в мед, остальные за мной. На месте разберёмся.",
        "Отлично, клоун нашёл гранату. Кто-нибудь, пожалуйста, заберите у него вторую.",
        "Ребят... я в морге. Тут один из мешков только что постучал изнутри.",
        "Шаттл пристыковался! Всё, бросай ящики и побежали, мы летим домой!"
    };

    private async void OnRequestPreviewTTS(RequestPreviewTTSEvent ev, EntitySessionEventArgs args)
    {
        if (!_isEnabled
            || !_prototypeManager.TryIndex<VoicePrototype>(ev.VoiceId, out var protoVoice)
            || protoVoice.ArcaneVoice is not { } speaker)
            return;

        var txt = _robustRandom.Pick(_sampleText);
        var cacheId = GetCacheId(protoVoice, $"{VoiceRequestType.Preview}-v4-{_sampleText.IndexOf(txt)}");

        var cached = await GetFromCache(cacheId);
        if (cached != null)
        {
            RaiseNetworkEvent(new PlayTTSEvent(cached), Filter.SinglePlayer(args.SenderSession));
            return;
        }

        var soundData = await GenerateTTS(txt, speaker);
        if (soundData is null)
            return;

        RaiseNetworkEvent(new PlayTTSEvent(soundData), Filter.SinglePlayer(args.SenderSession), false);

        await SaveVoiceCache(cacheId, soundData);
    }
}
