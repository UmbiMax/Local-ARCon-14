using Content.Client.Hands.Systems;
using Content.Client.NPC.HTN;
using Content.Shared.CCVar;
using Content.Shared.CombatMode;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Client.CombatMode;

public sealed partial class CombatModeSystem : SharedCombatModeSystem
{
    [Dependency] private IOverlayManager _overlayManager = default!;
    [Dependency] private IPlayerManager _playerManager = default!;
    [Dependency] private IConfigurationManager _cfg = default!;
    [Dependency] private IInputManager _inputManager = default!;
    [Dependency] private SharedAudioSystem _audio = default!; // Arcane
    [Dependency] private IEyeManager _eye = default!;

    // Arcane-Start
    private EntityUid? _combatModeSoundEntity;
    private bool? _lastCombatModeState;
    // Arcane-End

    /// <summary>
    /// Raised whenever combat mode changes.
    /// </summary>
    public event Action<bool>? LocalPlayerCombatModeUpdated;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CombatModeComponent, AfterAutoHandleStateEvent>(OnHandleState);

        Subs.CVar(_cfg, CCVars.CombatModeIndicatorsPointShow, OnShowCombatIndicatorsChanged, true);
    }

    private void OnHandleState(EntityUid uid, CombatModeComponent component, ref AfterAutoHandleStateEvent args)
    {
        UpdateHud(uid);
    }

    public override void Shutdown()
    {
        _overlayManager.RemoveOverlay<CombatModeIndicatorsOverlay>();

        base.Shutdown();
    }

    public bool IsInCombatMode()
    {
        var entity = _playerManager.LocalEntity;

        if (entity == null)
            return false;

        return IsInCombatMode(entity.Value);
    }

    public override void SetInCombatMode(EntityUid entity, bool value, CombatModeComponent? component = null)
    {
        base.SetInCombatMode(entity, value, component);
        UpdateHud(entity);
    }

    protected override bool IsNpc(EntityUid uid)
    {
        return HasComp<HTNComponent>(uid);
    }

    protected override bool ShouldShowCombatModePopup() => !_cfg.GetCVar(CCVars.CombatIndicator); // Arcane

    private void UpdateHud(EntityUid entity)
    {
        if (entity != _playerManager.LocalEntity || !Timing.IsFirstTimePredicted)
        {
            return;
        }

        var inCombatMode = IsInCombatMode();

        TryPlayCombatModeSound(entity, inCombatMode); // Arcane

        LocalPlayerCombatModeUpdated?.Invoke(inCombatMode);
    }

// Arcane-Start
    private void TryPlayCombatModeSound(EntityUid entity, bool inCombatMode)
    {
        if (_combatModeSoundEntity != entity)
        {
            _combatModeSoundEntity = entity;
            _lastCombatModeState = inCombatMode;
            return;
        }

        if (_lastCombatModeState == inCombatMode)
            return;

        _lastCombatModeState = inCombatMode;

        if (!_cfg.GetCVar(CCVars.CombatModeSoundEnabled))
            return;

        if (!TryComp(entity, out CombatModeComponent? component))
            return;

        var sound = inCombatMode
            ? component.CombatActivationSound
            : component.CombatDeactivationSound;

        _audio.PlayGlobal(sound, Filter.Local(), false);
    }
    // Arcane-End

    private void OnShowCombatIndicatorsChanged(bool isShow)
    {
        if (isShow)
        {
            _overlayManager.AddOverlay(new CombatModeIndicatorsOverlay(
                _inputManager,
                EntityManager,
                _eye,
                this,
                EntityManager.System<HandsSystem>()));
        }
        else
        {
            _overlayManager.RemoveOverlay<CombatModeIndicatorsOverlay>();
        }
    }
}
