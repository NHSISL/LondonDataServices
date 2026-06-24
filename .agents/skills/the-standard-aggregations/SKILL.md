---
name: the-standard-aggregations
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*AggregationService*.cs", "*ManagementService*.cs", "*AggregationServiceTests*.cs"]
depends-on: ["the-standard-core", "the-standard-orchestrations"]
---

# The Standard — Aggregation Services

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: The topmost business logic layer, above orchestration services.
0.1/ Who: Engineers implementing or reviewing aggregation (management) services.
0.2/ What: Enforces aggregation service design: fan-out coordination of multiple orchestration services with no data transformation.
0.3/ Applies to: *AggregationService*.cs, *ManagementService*.cs, *AggregationServiceTests*.cs
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-orchestrations

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Fan out calls to multiple orchestration services in sequence or in parallel → see rules/rules.md#ts-aggregations-001
  2. Expose a single entry-point method that triggers the aggregated workflow → see rules/rules.md#ts-aggregations-002
  3. Depend only on orchestration services (or processing services where no orchestration exists) → see rules/rules.md#ts-aggregations-003
  4. Return results only; never perform data transformation or business-rule enforcement → see rules/rules.md#ts-aggregations-004

1.1/ Don'ts:
  1. Must not contain business logic or validation beyond routing → see validations/anti-patterns.md#aggregation-with-logic
  2. Must not call foundation services or brokers directly → see validations/anti-patterns.md#aggregation-foundation-bypass
  3. Must not transform or map response models → see validations/anti-patterns.md#aggregation-mapping

1.2/ Ask:
  - Ask when the aggregation involves retry, compensation, or saga logic — those concerns require infrastructure broker support.

1.3/ Defaults:
  - When all orchestrations are independent, fan out in parallel using `Task.WhenAll`.
  - When orchestrations have ordering dependencies, execute sequentially and pass results forward.

1.4/ Examples:
  - ✅ see examples/good/example_good_aggregation_service.cs
  - ❌ see examples/bad/example_bad_aggregation_with_logic.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code.
2.1/ Outcome: Aggregation services that fan-out across orchestrations with no business logic or transformation.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
