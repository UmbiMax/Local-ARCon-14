using Robust.Shared.Configuration;

namespace Content.Shared._Arcane.CCVars;

public sealed partial class ACCVars
{
    /// <summary>
    ///     Whether TTS is enabled on the server.
    /// </summary>
    public static readonly CVarDef<bool> TTSEnabled =
        CVarDef.Create("atts.enabled", false, CVar.SERVER | CVar.REPLICATED | CVar.ARCHIVE);

    /// <summary>
    ///     URL of the TTS server API
    /// </summary>
    public static readonly CVarDef<string> TTSApiUrl =
        CVarDef.Create("atts.api_url", "", CVar.SERVERONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Auth token for the TTS server API
    /// </summary>
    ///
    public static readonly CVarDef<string> TTSApiToken =
        CVarDef.Create("atts.api_token", "", CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    ///     Amount of seconds before timeout for API.
    /// </summary>
    public static readonly CVarDef<int> TTSApiTimeout =
        CVarDef.Create("atts.api_timeout", 5, CVar.SERVERONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Default volume setting of TTS sound
    /// </summary>
    public static readonly CVarDef<float> TTSVolume =
        CVarDef.Create("atts.volume", 0.5f, CVar.CLIENTONLY | CVar.ARCHIVE); // Arcane

    /// <summary>
    ///     Count of in-memory cached tts voice lines.
    /// </summary>
    public static readonly CVarDef<int> TTSMaxCache =
        CVarDef.Create("atts.max_cache", 250, CVar.SERVERONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Громкость ТТСа рации.
    /// </summary>
    public static readonly CVarDef<float> TTSRadioVolume =
        CVarDef.Create("atts.radio_volume", 0.5f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Whether the client should play Arcane TTS audio.
    /// </summary>
    public static readonly CVarDef<bool> UseTTS =
        CVarDef.Create("atts.client_enabled", true, CVar.CLIENTONLY | CVar.ARCHIVE);
}
