# Unit tests

Unit tests exercise individual library rules with controlled inputs. The scaffold checks the current table column and row shape rules; it does not establish TypeScript compatibility.

With the .NET 10 SDK and access to the referenced NuGet packages, run from the repository root:

```sh
dotnet restore tests/IntelligentReportStudio.UnitTests/IntelligentReportStudio.UnitTests.csproj
dotnet test tests/IntelligentReportStudio.UnitTests/IntelligentReportStudio.UnitTests.csproj --no-restore
```

Add `--list-tests` to list discovered tests. The existing architecture test in this project also runs with this command and can report the known dependency-direction violations documented in `docs/architecture.md`. Test execution was not verified for this delivery.
