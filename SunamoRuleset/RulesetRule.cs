namespace SunamoRuleset;

public class RulesetRule
{
    public RulesetActions Action { get; set; } = RulesetActions.None;
    public string? Id { get; set; }

    public void Parse(XElement node)
    {
        Id = XHelper.Attr(node, "Id");
        Action = EnumHelper.Parse(XHelper.Attr(node, RulesetConsts.Action) ?? string.Empty, RulesetActions.None);
    }

    public string ToXml()
        => $"<Rule Id=\"{Id}\" Action=\"{Action}\" />";
}
