# The Standard Versioning — Rules

## PACKAGE VERSIONING

**ts-versioning-001** [ERROR] All NuGet packages must use Semantic Versioning: MAJOR.MINOR.PATCH.
**ts-versioning-002** [ERROR] MAJOR must be incremented for breaking changes; MINOR for backward-compatible new features; PATCH for backward-compatible bug fixes.
**ts-versioning-005** [ERROR] Every package release must have a corresponding CHANGELOG or release notes entry.

## BROKER CONTRACT VERSIONING (SPAL)

**ts-versioning-003** [ERROR] Breaking changes to broker contracts must use the SPAL strategy: introduce the new contract method alongside the old one; deprecate the old; remove only after all consumers have migrated.

## API VERSIONING

**ts-versioning-004** [ERROR] RESTful API version must be expressed in the URL path: `/api/v{n}/{resource}` (e.g., `/api/v1/students`). Query-string and header versioning must not be used as the primary mechanism.
