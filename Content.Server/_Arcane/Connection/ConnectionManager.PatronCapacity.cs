using System.Threading.Tasks;
using Content.Shared._NullLink;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Server.Connection;

public sealed partial class ConnectionManager
{
    private static readonly ProtoId<RoleRequirementPrototype> ArcanePatronRequirement = "ArcanePatronReq";

    private async Task<bool> HasArcanePatronCapacityBypass(NetUserId userId)
    {
        if (!_prototypeManager.TryIndex(ArcanePatronRequirement, out var requirement))
        {
            _sawmill.Error("Arcane patron role requirement {Requirement} is missing", ArcanePatronRequirement);
            return false;
        }

        try
        {
            return _actors.TryGetServerGrain(out var serverGrain)
                   && await serverGrain.HasPlayerAnyRole(userId, requirement.Roles);
        }
        catch (Exception exception)
        {
            _sawmill.Warning("Unable to check Arcane patron capacity bypass for {UserId}: {Exception}",
                userId,
                exception);
            return false;
        }
    }
}
