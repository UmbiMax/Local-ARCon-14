using Content.Shared._Arcane.ERP;
using Content.Shared.GameTicking;

namespace Content.Server._Arcane.ERP;

public sealed class ErpStatusSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(PlayerSpawnCompleteEvent args)
    {
        var preference = args.Profile.ErpPreference;
        var status = EnsureComp<ErpStatusComponent>(args.Mob);
        var oldPreference = status.Preference;

        status.Preference = preference;
        Dirty(args.Mob, status);

        if (oldPreference != preference)
            RaiseLocalEvent(args.Mob, new ErpPreferenceChangedEvent(oldPreference, preference));
    }
}
