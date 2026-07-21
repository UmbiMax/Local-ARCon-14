using Content.Shared._Arcane.ERP;

// ReSharper disable once CheckNamespace
namespace Content.Client.Lobby.UI;

public sealed partial class HumanoidProfileEditor
{
    private void InitializeErpPreferenceEditor()
    {
        foreach (var preference in Enum.GetValues<ErpPreference>())
        {
            var key = $"humanoid-profile-editor-erp-preference-{preference.ToString().ToLowerInvariant()}";
            ErpPreferenceButton.AddItem(Loc.GetString(key), (int) preference);
        }

        ErpPreferenceButton.OnItemSelected += args =>
        {
            ErpPreferenceButton.SelectId(args.Id);
            Profile = Profile?.WithErpPreference((ErpPreference) args.Id);
            SetDirty();
        };
    }

    private void UpdateErpPreferenceControls()
    {
        if (Profile == null)
            return;

        ErpPreferenceButton.SelectId((int) Profile.ErpPreference);
    }
}
