using Content.Client._Starlight.Character.Info.UI;
using Content.Client._Arcane.DetailExaminable;

// ReSharper disable once CheckNamespace
namespace Content.Client.UserInterface.Systems.Character;

public sealed partial class CharacterUIController
{
    private readonly Dictionary<EntityUid, DetailExaminableWindow> _openInspectionWindows = new(); // Arcane

    public void OpenInspectCharacterWindow(EntityUid target, EntityUid viewer)
    {
        if (!target.Valid)
            return;

        // Arcane: self-inspection uses the same detailed window as inspecting another character.

        if (_openInspectionWindows.TryGetValue(target, out var window))
        {
            window.SetCharacter(target, EntityManager, viewer);
            window.OpenCentered();
            return;
        }

        window = new DetailExaminableWindow(); // Arcane
        window.SetCharacter(target, EntityManager, viewer.Valid ? viewer : target);

        _openInspectionWindows[target] = window;

        window.OnClose += () => _openInspectionWindows.Remove(target);
        // Arcane-start
        window.Title = Loc.GetString("arcane-detail-examinable-window-title");
        window.OpenCentered();
        // Arcane-end
    }

    private void SLClearSelfCharacterInfo()
    {
        if (_window == null)
            return;
        _window.InfoIC.ClearCharacter();
        _window.InfoOOC.ClearCharacter();
        _window.InfoBackground.ClearCharacter();
    }

    private void SLSetSelfCharacterInfo()
    {
        if (_window == null)
            return;
        var ent = _window.CharacterInfo.CharacterPreview.CharacterSpriteView.Entity;
        if (!ent.HasValue)
        {
            _window.InfoIC.ClearCharacter();
            _window.InfoOOC.ClearCharacter();
            _window.InfoBackground.ClearCharacter();
            return;
        }

        _window.InfoIC.SetCharacter(ent, EntityManager, ent.Value);
        _window.InfoOOC.SetCharacter(ent, EntityManager, ent);
        _window.InfoBackground.SetCharacter(ent, EntityManager, ent);
    }
}
