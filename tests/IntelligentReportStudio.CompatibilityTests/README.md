# Compatibility tests

This .NET 10 xUnit project references the library and is discoverable by `dotnet test`. From the repository root, run:

```sh
dotnet restore tests/IntelligentReportStudio.CompatibilityTests/IntelligentReportStudio.CompatibilityTests.csproj
dotnet test tests/IntelligentReportStudio.CompatibilityTests/IntelligentReportStudio.CompatibilityTests.csproj --no-restore
```

Add `--list-tests` to the test command to list discovered tests. The location test checks only the fixture path convention; it does not compare engines or establish compatibility. Shared files under `fixtures/reference/` are copied to `fixtures/reference/` in the test output when present. The existing `schema-versions.json` contains synthetic inputs, not reference observations.

## Fixture convention

For each comparison case, maintain a record with the reference revision, input, expected observation, environment, and approval status. JSON values may be stored inline or as repository-relative artifact paths; record artifact hashes when external files are used. A missing value is explicitly `null`, never an invented observation.

```json
{
  "id": "CASE-ID",
  "feature": "Feature or JSON path under comparison",
  "reference": {
    "repository": "https://github.com/LuigimonSoft/IntelligentReportsStudio",
    "revision": null,
    "revisionApproval": { "status": "pending", "evidence": null }
  },
  "input": {
    "report": null,
    "data": null,
    "parameters": null,
    "provenance": "Describe the input source"
  },
  "expectedObservation": null,
  "environment": null,
  "observationApproval": { "status": "pending", "evidence": null }
}
```

`revision` is the exact full commit SHA used for capture. `input` records the complete report, data and parameters, or paths and hashes identifying them. The expected observation (`expectedObservation`) is the actual captured reference result or diagnostic, with its capture artifact and adapter provenance; it is not a prediction. `environment` records OS and architecture, Node.js and npm versions, locale, timezone, lockfile and adapter versions, and relevant fonts, dependencies and visual comparison settings. Record unknown details as `null` and retain the reason for any unavailable artifact. For visual cases, also record rasterizer version, resolution and agreed tolerances.

Approval statuses are `pending`, `approved`, or `rejected`; approval evidence identifies the user's decision and its location. Revision approval and observation approval are separate decisions. A change to the revision, input, observation, relevant environment, or comparison settings requires renewed review of the affected evidence. Do not put private data in fixtures.

A case is eligible for a compatibility comparison only when the exact reference revision and captured observation are available and separately approved by the user, and the input and relevant environment are reproducible. Pending, rejected, missing, or unavailable evidence cannot produce a verified compatibility result. A passing fixture comparison alone does not verify a feature: the specification, .NET implementation, relevant unit and integration tests, documented limitations, and user-approved comparison evidence are also required. Visual baselines require separate user approval. Record final feature verification in `docs/feature-matrix.md` under its established process.

No reference observation or approval is supplied by this convention. Capture instructions are in `tools/reference-capture/README.md`; ordinary tests do not run the TypeScript runtime.

The previous QA run used an empty package feed and failed to restore the test SDK and xUnit packages. An accessible package source or a populated NuGet cache is required. Test discovery and execution were not verified in this read-only environment.
