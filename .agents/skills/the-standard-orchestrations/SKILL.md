---
name: the-standard-orchestrations
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*OrchestrationService*.cs", "*OrchestrationServiceTests*.cs"]
depends-on: ["the-standard-core", "the-standard-foundations", "the-standard-processings"]
---

# The Standard — Orchestration Services

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: The coordination layer above foundation and processing services.
0.1/ Who: Engineers implementing or reviewing orchestration services.
0.2/ What: Enforces orchestration service design: multi-entity coordination, no broker access, correct dependency direction.
0.3/ Applies to: *OrchestrationService*.cs, *OrchestrationServiceTests*.cs
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-foundations, the-standard-processings

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Coordinate two or more foundation or processing services to fulfil a multi-entity business requirement → see rules/rules.md#ts-orchestrations-001
  2. Expose named business-operation methods that reflect the multi-entity workflow → see rules/rules.md#ts-orchestrations-002
  3. Depend only on foundation services, processing services, or other orchestration services → see rules/rules.md#ts-orchestrations-003
  4. Handle cross-entity validation and business rules at this layer → see rules/rules.md#ts-orchestrations-004
  5. Follow TDD: write a failing test before every behavior → see rules/rules.md#ts-orchestrations-005

1.1/ Don'ts:
  1. Must not call brokers directly → see validations/anti-patterns.md#broker-bypass
  2. Must not duplicate validation already enforced by the underlying services → see validations/anti-patterns.md#duplicate-validation
  3. Must not manage transactions or unit-of-work patterns directly → see validations/anti-patterns.md#direct-transaction-management

1.2/ Ask:
  - Ask when coordination spans more than three entity types — that may warrant a Management or Aggregation service.

1.3/ Defaults:
  - When orchestration involves an existence check across entities, delegate each entity check to its own foundation/processing service.

1.4/ Examples:
  - ✅ see examples/good/example_good_orchestration_service.cs
  - ❌ see examples/bad/example_bad_orchestration_broker_access.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code.
2.1/ Outcome: Orchestration services that coordinate multi-entity workflows through their respective services without broker access.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
