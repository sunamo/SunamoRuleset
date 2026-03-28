namespace SunamoRuleset;

/// <summary>
/// Represents the types of Roslyn analyzers supported in ruleset files.
/// </summary>
public enum RulesetTypes
{
    /// <summary>
    /// Microsoft Code Quality Analyzers (CA* rules for code quality).
    /// </summary>
    MicrosoftCodeQualityAnalyzers,

    /// <summary>
    /// Microsoft .NET Core Analyzers (CA* rules for .NET Core).
    /// </summary>
    MicrosoftNetCoreAnalyzers,

    /// <summary>
    /// Microsoft Code Analysis C# (CS* rules).
    /// </summary>
    MicrosoftCodeAnalysisCSharp,

    /// <summary>
    /// No analyzer type specified.
    /// </summary>
    None
}