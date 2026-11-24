# Behavior tests

Behavior tests describe consumer-observable outcomes using Given/When/Then. `DefinitionLoadingScenario.TemplateWithoutSchemaVersionCanBeLoadedAndCopied` is an executable xUnit test for the current .NET loading contract.

`Scenarios/DefinitionLoading.feature` documents the same scenario. Feature files are not parsed or executed by a Gherkin runner; keep the documented steps and xUnit test synchronized. This arrangement does not automatically detect missing tests or changes to feature files. It does not establish TypeScript compatibility.

With the .NET 10 SDK and access to the referenced NuGet packages, run from the repository root:

```sh
dotnet restore tests/IntelligentReportStudio.BehaviorTests/IntelligentReportStudio.BehaviorTests.csproj
dotnet test tests/IntelligentReportStudio.BehaviorTests/IntelligentReportStudio.BehaviorTests.csproj --no-restore
```

Add `--list-tests` to list discovered tests. Test execution was not verified for this delivery.
