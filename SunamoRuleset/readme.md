# SunamoRuleset

A .NET library for managing Visual Studio `.ruleset` files used by IDE analyzers, FxCop, ReSharper, and SonarQube.

## Features

- **Parse** existing `.ruleset` XML files into strongly-typed objects
- **Modify** rule actions programmatically
- **Save** changes back to `.ruleset` file format
- **Classify** rules by analyzer type (Microsoft Code Quality, .NET Core Analyzers, C# Code Analysis)

## Usage

```csharp
// Load a ruleset file
var manager = new RulesetManager("path/to/my.ruleset");

// Access rules by analyzer type
var codeQualityRules = manager.Rules[RulesetTypes.MicrosoftCodeQualityAnalyzers];

// Determine which analyzer owns a rule
var ruleType = RulesetManager.GetRuleType("CA1000");

// Save modifications
manager.Save();
```

## Target Frameworks

`net10.0;net9.0;net8.0`

## Links

- [NuGet](https://www.nuget.org/profiles/sunamo)
- [GitHub](https://github.com/sunamo/PlatformIndependentNuGetPackages)
- [Developer Site](https://sunamo.cz)

## Contact

Feature requests / bug reports: [Email](mailto:radek.jancik@sunamo.cz) or via GitHub Issues.

## License

MIT
