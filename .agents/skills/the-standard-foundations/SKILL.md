---
name: the-standard-foundations
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*Service*.cs", "*Tests*.cs"]
depends-on: ["the-standard-core", "the-standard-brokers"]
---

# The Standard — Foundation Services

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: The business logic layer, directly above brokers.
0.1/ Who: Engineers implementing or reviewing foundation (broker-neighboring) services.
0.2/ What: Enforces foundation service design: abstraction, validation, business language, and single-entity responsibility.
0.3/ Applies to: *Service*.cs, *Tests*.cs
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-brokers

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Translate broker storage language to business language: Insert→Add, Select→Retrieve, Update→Modify, Delete→Remove → see rules/rules.md#ts-foundations-001
  2. Expose only CRUD operations that the entity requires; no business-workflow methods → see rules/rules.md#ts-foundations-002
  3. Perform structural validation (null checks, empty Guid, empty strings) on all inputs → see rules/rules.md#ts-foundations-003
  4. Perform logical validation (date ranges, referential integrity) where applicable → see rules/rules.md#ts-foundations-004
  5. Integrate with exactly one entity broker only → see rules/rules.md#ts-foundations-005
  6. Wrap all broker and unexpected exceptions in service-specific exception models → see rules/rules.md#ts-foundations-006
  7. Write a failing test before implementing every behavior → see rules/rules.md#ts-foundations-007
  8. Randomize all test inputs and outputs; use `DeepClone()` for expected variables → see rules/rules.md#ts-foundations-008

1.1/ Don'ts:
  1. Must not call more than one entity broker → see validations/anti-patterns.md#multi-broker-foundation
  2. Must not implement business-workflow methods (e.g., Upsert) — that belongs in Processing services → see validations/anti-patterns.md#workflow-in-foundation
  3. Must not expose raw broker exceptions to consumers → see validations/anti-patterns.md#naked-broker-exceptions
  4. Must not use specific values in tests where randomization is possible → see validations/anti-patterns.md#hardcoded-test-values

1.2/ Ask:
  - Ask when a validation rule requires knowledge of another entity (e.g., "does this teacher exist?") — that likely belongs in an Orchestration service.

1.3/ Defaults:
  - Default to structural validation first, then logical validation.
  - Default exception wrapping: broker exceptions → dependency exceptions; unexpected exceptions → service exceptions.

1.4/ Examples:
  - ✅ see examples/good/example_good_foundation_service.cs
  - ❌ see examples/bad/example_bad_foundation_no_validation.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code.
2.1/ Outcome: Foundation services that abstract broker operations, validate inputs, use business language, and handle exceptions correctly.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
