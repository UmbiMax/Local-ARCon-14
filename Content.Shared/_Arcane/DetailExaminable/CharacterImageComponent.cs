using Robust.Shared.GameStates;

namespace Content.Shared._Arcane.DetailExaminable;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CharacterImageComponent : Component
{
    [DataField, AutoNetworkedField]
    public string ImageUrl = string.Empty;
}
