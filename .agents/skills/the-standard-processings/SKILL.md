---
name: the-standard-processings
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*ProcessingService*.cs", "*ProcessingServiceTests*.cs"]
depends-on: ["the-standard-core", "the-standard-foundations"]
---

# The Standard — Processing Services

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: The higher-order business logic layer, directly above foundation services.
0.1/ Who: Engineers implementing or reviewing processing services.
0.2/ What: Enforces processing service design: combination of foundation primitives, business workflows, and no direct broker access.
0.3/ Applies to: *ProcessingService*.cs, *ProcessingServiceTests*.cs
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-foundations

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Combine two or more foundation service primitives to form a business workflow → see rules/rules.md#ts-processings-001
  2. Expose higher-order business verbs: Ensure, Process, Calculate, Upsert → see rules/rules.md#ts-processings-002
  3. Depend only on foundation services (or other processing services); never depend on brokers directly → see rules/rules.md#ts-processings-003
  4. Handle cross-cutting concerns (e.g., existence checks) within the processing service → see rules/rules.md#ts-processings-004
  5. Follow TDD: write a failing test before every behavior → see rules/rules.md#ts-processings-005

1.1/ Don'ts:
  1. Must not call brokers directly → see validations/anti-patterns.md#broker-bypass
  2. Must not duplicate validation already performed by the underlying foundation service → see validations/anti-patterns.md#duplicate-validation
  3. Must not expose raw CRUD primitives (Add, Retrieve, Modify, Remove) as the primary API → see validations/anti-patterns.md#crud-exposure

1.2/ Ask:
  - Ask when a workflow requires data from more than two entities — that likely belongs in an Orchestration service.

1.3/ Defaults:
  - When a processing service checks existence before writing, it calls `Retrieve` then `Add` or `Modify` — not a custom broker query.

1.4/ Examples:
  - ✅ see examples/good/example_good_processing_service.cs
  - ❌ see examples/bad/example_bad_processing_direct_broker.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code.
2.1/ Outcome: Processing services that combine foundation primitives into named business workflows without broker access.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
