---
applyTo: "**/*.cs,**/*.csproj"
---

# The Standard — Versioning

## Applies To
All projects — broker contracts, NuGet package versions, and API versioning (`*.cs`, `*.csproj`).

## Rules — Do
- Apply Semantic Versioning (SemVer) to all NuGet packages: MAJOR.MINOR.PATCH (ts-versioning-001)
- Increment MAJOR for breaking changes; MINOR for new features; PATCH for fixes (ts-versioning-002)
- Use the SPAL strategy for broker contract versioning: maintain the old contract until all consumers migrate (ts-versioning-003)
- Version RESTful API endpoints using URL path versioning: `/api/v1/students`, `/api/v2/students` (ts-versioning-004)
- Record version history in a CHANGELOG or release notes (ts-versioning-005)

## Rules — Do Not
- Must not remove or rename a broker contract method without providing a migration path (ts-versioning-001)
- Must not version APIs using query strings or headers as the primary versioning mechanism (ts-versioning-002)
- Must not release a package with version `0.0.0` or without a version number (ts-versioning-003)

## Defaults
- New projects start at version `0.1.0`.
- First stable public release is `1.0.0`.
- Pre-release packages use the `-preview.{n}` suffix — e.g., `1.0.0-preview.1`.
