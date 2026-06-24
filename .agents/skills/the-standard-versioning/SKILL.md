---
name: the-standard-versioning
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*.cs", "*.csproj"]
depends-on: ["the-standard-core", "the-standard-brokers"]
---

# The Standard — Versioning

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All Standard-compliant projects — broker contracts, package versions, and API versioning.
0.1/ Who: Engineers managing breaking changes, NuGet package releases, and API versioning.
0.2/ What: Enforces SPAL versioning for broker contracts, semantic versioning for packages, and API version routing.
0.3/ Applies to: *.cs, *.csproj
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-brokers

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Apply Semantic Versioning (SemVer) to all NuGet packages: MAJOR.MINOR.PATCH → see rules/rules.md#ts-versioning-001
  2. Increment MAJOR when a breaking change is introduced; MINOR for new features; PATCH for fixes → see rules/rules.md#ts-versioning-002
  3. Use the SPAL strategy for broker contract versioning: maintain the old contract until all consumers migrate → see rules/rules.md#ts-versioning-003
  4. Version RESTful API endpoints using URL path versioning: `/api/v1/students`, `/api/v2/students` → see rules/rules.md#ts-versioning-004
  5. Record version history in a CHANGELOG or release notes → see rules/rules.md#ts-versioning-005

1.1/ Don'ts:
  1. Must not remove or rename a broker contract method without providing a migration path → see validations/anti-patterns.md#breaking-contract-change
  2. Must not version APIs using query strings or headers as the primary versioning mechanism → see validations/anti-patterns.md#querystring-versioning
  3. Must not release a package with version `0.0.0` or without a version number → see validations/anti-patterns.md#missing-version

1.2/ Ask:
  - Ask when a change to a broker method signature is required — confirm whether SPAL migration is needed.

1.3/ Defaults:
  - New projects start at version `0.1.0`.
  - First stable public release is `1.0.0`.
  - Pre-release packages use `-preview.{n}` suffix (e.g., `1.0.0-preview.1`).

1.4/ Examples:
  - ✅ see examples/good/example_good_versioned_api.cs
  - ❌ see examples/bad/example_bad_querystring_versioning.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code, `.csproj` XML, API route attributes.
2.1/ Outcome: Correctly versioned packages, broker contracts, and API endpoints with documented migration paths.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
