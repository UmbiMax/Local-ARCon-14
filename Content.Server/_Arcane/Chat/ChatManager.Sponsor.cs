using Content.Server._Arcane;
using Robust.Shared.Player;

namespace Content.Server.Chat.Managers;

internal sealed partial class ChatManager
{
    private const ulong ArcaneSponsorTierOneRole = 1510991486399942707;
    private const ulong ArcaneSponsorTierTwoRole = 1510991694785675397;

    private static readonly Color ArcaneSponsorTierOneColor = Color.FromHex("#8b00d1");
    private static readonly Color ArcaneSponsorTierTwoColor = Color.FromHex("#ecad00");

    [Dependency] private readonly IDiscordOAuthManager _arcaneDiscordOAuth = default!;

    private Color? GetArcaneSponsorNameColor(ICommonSession player)
    {
        if (!_arcaneDiscordOAuth.TryGetRoles(player, out var roles))
            return null;

        if (roles.Contains(ArcaneSponsorTierTwoRole))
            return ArcaneSponsorTierTwoColor;

        return roles.Contains(ArcaneSponsorTierOneRole)
            ? ArcaneSponsorTierOneColor
            : null;
    }
}
