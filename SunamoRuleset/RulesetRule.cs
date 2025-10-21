// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRuleset;

public class RulesetRule
{
    public RulesetActions Action = RulesetActions.None;
    public string Id;
    public void Parse(XElement node)
    {
        Id = XHelper.Attr(node, "Id");
        Action = EnumHelper.Parse(XHelper.Attr(node, RulesetConsts.Action), RulesetActions.None);
    }
    public string ToXml()
    {
        return "<Rule Id=\"" + Id + "\" Action=\"" + Action + "\" />";
    }
}