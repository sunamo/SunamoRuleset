namespace SunamoRuleset;

/// <summary>
/// Represents the ruleset rule type.
/// </summary>
public class RulesetRule
{
    /// <summary>
    /// Gets or sets the action.
    /// </summary>
    public RulesetActions Action { get; set; } = RulesetActions.None;
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Parse.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>The parse result.</returns>
    public void Parse(XElement node)
    {
        Id = XHelper.Attr(node, "Id");
        Action = EnumHelper.Parse(XHelper.Attr(node, RulesetConsts.Action) ?? string.Empty, RulesetActions.None);
    }

    /// <summary>
    /// To xml.
    /// </summary>
    /// <param name="node">The node.</param>
    public string ToXml()
        => $"<Rule Id=\"{Id}\" Action=\"{Action}\" />";
}
