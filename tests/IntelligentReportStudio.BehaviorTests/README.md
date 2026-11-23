# Behavior tests

This .NET 10 project uses xUnit as an executable Given/When/Then scenario arrangement. The `.feature` files document scenarios; they are not parsed or executed by a Gherkin runner. Each scenario must have a corresponding xUnit test whose Given, When, and Then sections implement the documented steps. Keep the feature and test synchronized when changing either one.

`Scenarios/DefinitionLoading.feature` maps to `DefinitionLoadingScenario.TemplateWithoutSchemaVersionCanBeLoadedAndCopied`. The scenario checks the current .NET loading contract; it does not establish compatibility with the TypeScript runtime.

With the .NET 10 SDK and access to the referenced NuGet packages, run from the repository root:

```sh
dotnet restore tests/IntelligentReportStudio.BehaviorTests/IntelligentReportStudio.BehaviorTests.csproj
dotnet test tests/IntelligentReportStudio.BehaviorTests/IntelligentReportStudio.BehaviorTests.csproj --no-restore
```

The project uses Microsoft.NET.Test.Sdk and the xUnit Visual Studio runner for discovery by `dotnet test`. New scenarios can follow the same arrangement without adding a Gherkin dependency. This arrangement does not automatically detect missing tests or changes to feature files.

The previous QA run used an empty package feed and failed to restore the test SDK and xUnit packages. An empty feed can restore these dependencies only if they are already in the NuGet package cache. The test was not executed in this read-only environment.
