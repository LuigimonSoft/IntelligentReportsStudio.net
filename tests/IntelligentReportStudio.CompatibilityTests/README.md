# Compatibility tests

Compatibility tests are intended to compare .NET behavior with separately captured and approved reference observations. The scaffold checks only the fixture path convention; it does not compare engines or establish compatibility. Shared files under `fixtures/reference/` are copied to the test output when present. The existing `schema-versions.json` contains synthetic inputs, not reference observations.

Reference revision selection and observation approvals are pending. No approved comparison cases or parity are claimed. A comparison requires a reproducible input, exact reference revision, captured observation, relevant environment and separate user approvals. See `docs/reference-revisions.md`, `docs/feature-matrix.md` and `tools/reference-capture/README.md` for the evidence and approval process.

With the .NET 10 SDK and access to the referenced NuGet packages, run from the repository root:

```sh
dotnet restore tests/IntelligentReportStudio.CompatibilityTests/IntelligentReportStudio.CompatibilityTests.csproj
dotnet test tests/IntelligentReportStudio.CompatibilityTests/IntelligentReportStudio.CompatibilityTests.csproj --no-restore
```

Add `--list-tests` to list discovered tests. Ordinary tests do not run the TypeScript runtime. Test execution was not verified for this delivery.
