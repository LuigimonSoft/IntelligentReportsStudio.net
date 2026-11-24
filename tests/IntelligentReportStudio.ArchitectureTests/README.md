# Architecture tests

This xUnit project checks the library's dependency boundaries. With the .NET 10 SDK and access to the referenced NuGet packages, run from the repository root:

```sh
dotnet restore tests/IntelligentReportStudio.ArchitectureTests/IntelligentReportStudio.ArchitectureTests.csproj
dotnet test tests/IntelligentReportStudio.ArchitectureTests/IntelligentReportStudio.ArchitectureTests.csproj --no-restore
```

Use `dotnet test tests/IntelligentReportStudio.ArchitectureTests/IntelligentReportStudio.ArchitectureTests.csproj --no-restore --list-tests` to list discovered checks.

The check compiles the library source with Roslyn and resolves type references in declarations and method bodies. This includes signatures, base types, interfaces and generic arguments. Source directories determine layers, including for types in the public root namespace. Models cannot depend on Application, Services or Repository, and input and output models cannot depend on one another. Services cannot depend on Application or Repository; Repository cannot depend on Application and may implement service-owned ports. The check also detects references to common graphical framework namespaces and assemblies. It reports the source location and target type for violations.

The analysis recreates the current SDK implicit imports and the library's service-contract import. Update these imports and the reference setup if the library project changes its compilation configuration or adds dependencies. Reflection, dynamically loaded assemblies, native dependencies and frameworks outside the listed namespace and assembly prefixes are not covered.

The supplied library currently has boundary violations: contracts in `Models/Output/ServiceContracts.cs` reference the input model and Application snapshots, and the service binder references Application snapshots. The check reports these until their owning activities correct the production code; it does not suppress them. Consequently, a clean run cannot be expected with the supplied source. Test execution was not verified in the read-only environment.
