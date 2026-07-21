namespace Content.Shared._Arcane.ERP
{
    public static class ErpPreferenceHelpers
    {
        public static ErpPreference EnsureValid(this ErpPreference preference)
        {
            return preference switch
            {
                ErpPreference.No => ErpPreference.No,
                ErpPreference.Ask => ErpPreference.Ask,
                ErpPreference.Yes => ErpPreference.Yes,
                _ => ErpPreference.Ask,
            };
        }
    }
}

// ReSharper disable once CheckNamespace
namespace Content.Shared.Preferences
{
    using Content.Shared._Arcane.ERP;

    public sealed partial class HumanoidCharacterProfile
    {
        [DataField]
        public ErpPreference ErpPreference { get; set; } = ErpPreference.Ask;

        public HumanoidCharacterProfile WithErpPreference(ErpPreference preference)
        {
            return new(this) { ErpPreference = preference.EnsureValid() };
        }
    }
}
