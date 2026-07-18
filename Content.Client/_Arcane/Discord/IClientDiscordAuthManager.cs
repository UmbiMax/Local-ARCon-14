namespace Content.Client._Arcane;

public interface IClientDiscordOAuthManager
{
    void RequestLink();

    bool ContainsAny(ulong[] roles);

    void Initialize();
}
