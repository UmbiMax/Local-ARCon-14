using Content.Shared._Arcane.ERP;
using Content.Shared.Examine;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Server._Arcane.ERP;

public sealed class ErpExamineSystem : EntitySystem
{
    [Dependency] private readonly ExamineSystemShared _examine = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ErpStatusComponent, GetVerbsEvent<ExamineVerb>>(OnGetExamineVerbs);
    }

    private void OnGetExamineVerbs(Entity<ErpStatusComponent> ent, ref GetVerbsEvent<ExamineVerb> args)
    {
        var user = args.User;
        var inRange = _examine.IsInDetailsRange(user, ent);

        args.Verbs.Add(new ExamineVerb
        {
            Act = () => ShowStatus(user, ent),
            Text = Loc.GetString("erp-examine-verb-text"),
            Category = VerbCategory.Examine,
            Disabled = !inRange,
            Message = inRange ? null : Loc.GetString("erp-examine-verb-disabled"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/_Arcane/Interface/heartIcon.png")),
        });
    }

    private void ShowStatus(EntityUid user, Entity<ErpStatusComponent> target)
    {
        var statusKey = target.Comp.Preference switch
        {
            ErpPreference.Yes => "erp-examine-status-yes",
            ErpPreference.Ask => "erp-examine-status-ask",
            _ => "erp-examine-status-no",
        };

        var message = new FormattedMessage();
        message.AddMarkupOrThrow(Loc.GetString("erp-examine-status-header"));
        message.PushNewline();
        message.AddMarkupOrThrow(Loc.GetString(statusKey));
        _examine.SendExamineTooltip(user, target, message, false, false);
    }
}
