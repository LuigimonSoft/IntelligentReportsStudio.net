# Integration tests

Integration tests exercise library boundaries and interactions. The scaffold loads a minimal definition through the public stream API and checks that the caller-owned stream remains open. It does not start a UI or JavaScript runtime.

With the .NET 10 SDK and access to the referenced NuGet packages, run from the repository root:

```sh
dotnet restore tests/IntelligentReportStudio.IntegrationTests/IntelligentReportStudio.IntegrationTests.csproj
dotnet test tests/IntelligentReportStudio.IntegrationTests/IntelligentReportStudio.IntegrationTests.csproj --no-restore
```

Add `--list-tests` to list discovered tests. Test execution was not verified for this delivery.
