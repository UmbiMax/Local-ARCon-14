using Content.Shared._Starlight.Language;
using Robust.Shared.Player;

namespace Content.Shared._Arcane.TTS;

public sealed class TTSRadioPlayEvent(Filter filter, string message, LanguagePrototype language, string voice) : EntityEventArgs
{
    public Filter Recievers { get; } = filter;
    public string Message { get; } = message;
    public LanguagePrototype Language { get; } = language;
    public string Voice { get; } = voice;
}

public sealed class TTSAnnouncePlayEvent(string message, EntityUid? sender, Filter filter) : EntityEventArgs
{
    public string Message { get; } = message;
    public EntityUid? Sender { get; } = sender;
    public Filter Recievers { get; } = filter;
}
