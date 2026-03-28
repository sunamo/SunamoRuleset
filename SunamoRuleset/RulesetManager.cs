namespace SunamoRuleset;

/// <summary>
/// Manages loading, parsing, and saving of Visual Studio ruleset files.
/// Supports Microsoft Code Quality, .NET Core, and C# Code Analysis rule types.
/// </summary>
public class RulesetManager
{
    private readonly string description;
    private readonly string name;
    private readonly string rulesetPath;
    private readonly string toolsVersion;

    /// <summary>
    /// Dictionary of parsed rules grouped by their analyzer type.
    /// </summary>
    public Dictionary<RulesetTypes, List<RulesetRule>> Rules { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="RulesetManager"/> class by parsing a ruleset file.
    /// </summary>
    /// <param name="rulesetPath">The full path to the .ruleset file to parse.</param>
    public RulesetManager(string rulesetPath)
    {
        this.rulesetPath = rulesetPath;
        var content = File.ReadAllText(rulesetPath);
        var document = XDocument.Parse(content);
        var root = document.Root!;
        var rulesElements = root.Descendants().Where(element => element.Name == "Rules");
        var rulesetType = RulesetTypes.None;
        name = XHelper.Attr(root, "Name") ?? string.Empty;
        description = XHelper.Attr(root, "Description") ?? string.Empty;
        toolsVersion = XHelper.Attr(root, "ToolsVersion") ?? string.Empty;
        var unrecognizedRules = new List<string>();
        foreach (var rulesElement in rulesElements)
        {
            var analyzerId = AttrRules(rulesElement, "AnalyzerId");
            rulesetType = EnumHelper.Parse(analyzerId, RulesetTypes.None);
            if (rulesetType == RulesetTypes.None)
            {
                var ruleNamespace = AttrRules(rulesElement, "RuleNamespace");
                rulesetType = EnumHelper.Parse(ruleNamespace, RulesetTypes.None);
            }
            if (rulesetType == RulesetTypes.None)
            {
                unrecognizedRules.Add(rulesElement.ToString());
                continue;
            }
            var ruleElements = rulesElement.Descendants().Where(element => element.Name == "Rule");
            foreach (var ruleElement in ruleElements)
            {
                var rulesetRule = new RulesetRule();
                rulesetRule.Parse(ruleElement);
                DictionaryHelper.AddOrCreate(Rules, rulesetType, rulesetRule);
            }
        }
    }

    private string AttrRules(XElement element, string attributeName)
    {
        return (XHelper.Attr(element, attributeName) ?? string.Empty).Replace(".", string.Empty);
    }

    /// <summary>
    /// Determines the analyzer type for a given rule code based on its prefix or known rule lists.
    /// </summary>
    /// <param name="rule">The rule code (e.g., "CS0168", "CA1000").</param>
    /// <returns>The <see cref="RulesetTypes"/> corresponding to the rule code.</returns>
    public static RulesetTypes GetRuleType(string rule)
    {
        if (rule.StartsWith("CS")) return RulesetTypes.MicrosoftCodeAnalysisCSharp;
        if (RulesetValues.RulesMicrosoftNetCoreAnalyzers.Contains(rule))
            return RulesetTypes.MicrosoftNetCoreAnalyzers;
        if (RulesetValues.RulesMicrosoftCodeQuality.Contains(rule)) return RulesetTypes.MicrosoftCodeQualityAnalyzers;
        return RulesetTypes.None;
    }

    /// <summary>
    /// Converts a <see cref="RulesetTypes"/> enum value to its dot-separated analyzer namespace string.
    /// </summary>
    /// <param name="rulesetType">The ruleset type to convert.</param>
    /// <returns>The dot-separated namespace string, or null for <see cref="RulesetTypes.None"/>.</returns>
    public string? ConvertToDotSyntax(RulesetTypes rulesetType)
    {
        switch (rulesetType)
        {
            case RulesetTypes.MicrosoftCodeQualityAnalyzers:
                return "Microsoft.CodeQuality.Analyzers";
            case RulesetTypes.MicrosoftNetCoreAnalyzers:
                return "Microsoft.NetCore.Analyzers";
            case RulesetTypes.MicrosoftCodeAnalysisCSharp:
                return "Microsoft.CodeAnalysis.CSharp";
            case RulesetTypes.None:
                return null;
            default:
                ThrowEx.NotImplementedCase(rulesetType);
                break;
        }
        return null;
    }

    /// <summary>
    /// Saves the current ruleset configuration back to the file it was loaded from.
    /// </summary>
    public void Save()
    {
        var xmlGenerator = new XmlGenerator();
        xmlGenerator.WriteXmlDeclaration();
        xmlGenerator.WriteTagWithAttrs(RulesetConsts.RuleSet, RulesetConsts.Name, name, RulesetConsts.Description, description,
            RulesetConsts.ToolsVersion, toolsVersion);
        foreach (var item in Rules)
        {
            var dotSyntax = ConvertToDotSyntax(item.Key);
            xmlGenerator.WriteTagWithAttrs(RulesetConsts.Rules, RulesetConsts.AnalyzerId, dotSyntax!, RulesetConsts.RuleNamespace,
                dotSyntax!);
            foreach (var rule in item.Value) xmlGenerator.WriteRaw(rule.ToXml());
            xmlGenerator.TerminateTag(RulesetConsts.Rules);
        }
        xmlGenerator.TerminateTag(RulesetConsts.RuleSet);
        File.WriteAllText(rulesetPath, xmlGenerator.ToString());
    }
}
