---
name: the-standard-exceptions
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*Exception*.cs"]
depends-on: ["the-standard-core", "the-standard-foundations"]
---

# The Standard — Exceptions

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All layers of Standard-compliant systems — exception model definitions and usage.
0.1/ Who: Engineers defining exception models or reviewing exception handling code.
0.2/ What: Enforces the exception hierarchy, wrapping/unwrapping rules, visibility rules, and mapping between layers.
0.3/ Applies to: *Exception*.cs
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-foundations

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Define a three-category exception hierarchy per service: Validation, Dependency, Service → see rules/rules.md#ts-exceptions-001
  2. Wrap all broker/infrastructure exceptions as `{Entity}DependencyException` → see rules/rules.md#ts-exceptions-002
  3. Wrap all unexpected exceptions as `{Entity}ServiceException` → see rules/rules.md#ts-exceptions-003
  4. Wrap all validation failures as `{Entity}ValidationException` → see rules/rules.md#ts-exceptions-004
  5. Use `Xeptions` (or equivalent) as the base class for all custom exceptions → see rules/rules.md#ts-exceptions-005
  6. Pass the inner (original) exception as the `innerException` to all wrapping exceptions → see rules/rules.md#ts-exceptions-006
  7. Include the entity name in every exception class name → see rules/rules.md#ts-exceptions-007

1.1/ Don'ts:
  1. Must not let infrastructure exceptions (SqlException, HttpRequestException) propagate beyond the foundation service → see validations/anti-patterns.md#naked-infrastructure-exception
  2. Must not create generic exceptions (e.g., `AppException`, `ServiceException`) without an entity qualifier → see validations/anti-patterns.md#generic-exception-name
  3. Must not throw the raw `Exception` base class → see validations/anti-patterns.md#raw-exception-throw
  4. Must not catch-all with an empty handler that swallows exceptions silently → see validations/anti-patterns.md#swallowed-exception

1.2/ Ask:
  - Ask when an exception crosses more than two layer boundaries — confirm whether re-wrapping is required at each boundary.

1.3/ Defaults:
  - Dependency validation exceptions (e.g., duplicate key) → `{Entity}DependencyValidationException`.
  - Not-found after a retrieval → `NotFound{Entity}Exception` wrapped in `{Entity}ValidationException`.

1.4/ Examples:
  - ✅ see examples/good/example_good_exception_hierarchy.cs
  - ❌ see examples/bad/example_bad_naked_exception.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code.
2.1/ Outcome: Exception models that are named, scoped, layered, and wrapped correctly so callers receive meaningful, actionable error information.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
