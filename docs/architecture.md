# Architecture

This delivery provides solution scaffolding for a headless .NET 10 reporting library. Definition loading and basic table construction are available, but the reporting engine is incomplete: binding, layout, and PDF rendering fail explicitly. No TypeScript compatibility is verified.

## Library areas and dependency rules

The solution has a single library project, `src/IntelligentReportStudio/IntelligentReportStudio.csproj`. Its source directories define five architectural areas, even when public types use the root `IntelligentReportStudio` namespace.

| Area | Responsibility | Allowed dependencies |
|---|---|---|
| `Application` | Public request and generation entry points, input snapshots, and orchestration; the intended composition root. | Models, Services, and Repository. |
| `Services` | Validation, binding, relations, expressions, layout, and ports for operations supplied by adapters. | Models and other service contracts. |
| `Repository` | JSON and file access, resource access, and PDF adapters. | Models and service-owned ports. |
| `Models/Input` | Report definitions and other input models. | The .NET base class library and other input models. |
| `Models/Output` | Diagnostics and generated-document models. | The .NET base class library and other output models. |

Input and output models must not depend on each other. Models must not depend on Application, Services, or Repository. Services must not depend on Application or Repository; Repository must not depend on Application. Repository adapters may implement service-owned ports. The library must remain independent of graphical application frameworks.

Service-owned ports are intended to live under `Services`, such as `Services/Ports`. The existing internal interfaces and shared contract types are currently in `Models/Output/ServiceContracts.cs`, although they use the `IntelligentReportStudio.Services` namespace. Architecture checks classify them by their physical location, so they currently belong to the output-model boundary. That file references the input model and snapshots in `Application/Data/InputSnapshots.cs`; the service binder also references those Application snapshots. These are known violations of the intended dependency direction, not exceptions to it. Their relocation belongs to the activities responsible for those sources.

## Solution projects

`IntelligentReportStudio.slnx` includes the library and the following projects. The example and each test project reference the library; test projects do not reference one another.

| Project | Purpose |
|---|---|
| `src/IntelligentReportStudio.Examples` | Example scaffold; PDF generation is not available. |
| `tests/IntelligentReportStudio.UnitTests` | Individual library rules with controlled inputs. |
| `tests/IntelligentReportStudio.IntegrationTests` | Interactions at library boundaries, including stream loading. |
| `tests/IntelligentReportStudio.BehaviorTests` | Executable consumer scenarios and accompanying human-readable feature descriptions. |
| `tests/IntelligentReportStudio.CompatibilityTests` | Scaffold for future comparisons with approved reference observations. |
| `tests/IntelligentReportStudio.ArchitectureTests` | Dependency direction and graphical-framework checks. |

## Architecture tests

Because the boundaries are inside one library project, project references alone cannot enforce them. The dedicated architecture test compiles the library source with Roslyn, classifies declarations by source directory, and resolves type references, including signatures, base types, interfaces, generic arguments, and method bodies. It reports forbidden source and target dependencies and checks for common graphical framework namespaces and assemblies. Framework types otherwise fall outside the layer rules. The check permits Repository implementations to depend on service-owned ports; it does not restrict Repository dependencies on Services to ports alone.

The current source has the known violations described above, so the dedicated architecture test is expected to report them until the production code is corrected. The unit test project also contains an architecture check with a narrower set of rules. Unused project or package references, reflection, dynamically loaded assemblies, native dependencies, and graphical frameworks outside the dedicated check's listed prefixes are not covered. See [the architecture test README](../tests/IntelligentReportStudio.ArchitectureTests/README.md) for details and limitations.

With the .NET 10 SDK and access to the required NuGet packages, run from the repository root:

```sh
dotnet restore IntelligentReportStudio.slnx
dotnet test IntelligentReportStudio.slnx --no-restore
```

Ordinary tests do not run the TypeScript runtime. The compatibility scaffold copies reference fixtures when present; its current synthetic inputs are not approved reference observations. See [the feature matrix](feature-matrix.md) and [reference revisions](reference-revisions.md) for compatibility evidence and pending decisions.
