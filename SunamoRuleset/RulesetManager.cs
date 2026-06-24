namespace SunamoRuleset;

public class RulesetManager
{
    private readonly string description;
    private readonly string name;
    private readonly string rulesetPath;
    private readonly string toolsVersion;

    public Dictionary<RulesetTypes, List<RulesetRule>> Rules { get; set; } = new();

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
        => (XHelper.Attr(element, attributeName) ?? string.Empty).Replace(".", string.Empty);

    public static RulesetTypes GetRuleType(string rule)
    {
        if (rule.StartsWith("CS")) return RulesetTypes.MicrosoftCodeAnalysisCSharp;
        if (RulesetValues.RulesMicrosoftNetCoreAnalyzers.Contains(rule))
            return RulesetTypes.MicrosoftNetCoreAnalyzers;
        if (RulesetValues.RulesMicrosoftCodeQuality.Contains(rule)) return RulesetTypes.MicrosoftCodeQualityAnalyzers;
        return RulesetTypes.None;
    }

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
