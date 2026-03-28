namespace SunamoRuleset;

/// <summary>
/// Represents a single rule within a ruleset file, with an ID and action.
/// </summary>
public class RulesetRule
{
    /// <summary>
    /// The action to take for this rule (e.g., None, Warning, Error).
    /// </summary>
    public RulesetActions Action { get; set; } = RulesetActions.None;

    /// <summary>
    /// The unique identifier of the rule (e.g., CA1000, CS0168).
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Parses rule attributes from an XML element.
    /// </summary>
    /// <param name="node">The XML element representing the rule.</param>
    public void Parse(XElement node)
    {
        Id = XHelper.Attr(node, "Id");
        Action = EnumHelper.Parse(XHelper.Attr(node, RulesetConsts.Action) ?? string.Empty, RulesetActions.None);
    }

    /// <summary>
    /// Converts this rule to its XML string representation.
    /// </summary>
    /// <returns>An XML string representing this rule.</returns>
    public string ToXml()
    {
        return "<Rule Id=\"" + Id + "\" Action=\"" + Action + "\" />";
    }
}
