// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRuleset;

public enum RulesetTypes
{
    /// <summary>
    ///     codesMicrosoftCodeQualityRules
    /// </summary>
    MicrosoftCodeQualityAnalyzers,

    /// <summary>
    ///     rulesMicrosoftNetCoreAnalyzers
    /// </summary>
    MicrosoftNetCoreAnalyzers,

    /// <summary>
    ///     CS*
    /// </summary>
    MicrosoftCodeAnalysisCSharp,
    None
}