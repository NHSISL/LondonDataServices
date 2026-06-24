---
applyTo: "**/*.cs"
---

# The Standard — Aggregation Services

## Applies To
The topmost business logic layer — `*AggregationService*.cs`, `*ManagementService*.cs`, `*AggregationServiceTests*.cs`.

## Rules — Do
- Fan out calls to multiple orchestration services in sequence or in parallel (ts-aggregations-001)
- Expose a single entry-point method that triggers the aggregated workflow (ts-aggregations-002)
- Depend only on orchestration services, or processing services where no orchestration exists (ts-aggregations-003)
- Return results only; never perform data transformation or business-rule enforcement (ts-aggregations-004)

## Rules — Do Not
- Must not contain business logic or validation beyond routing (ts-aggregations-001)
- Must not call foundation services or brokers directly (ts-aggregations-002)
- Must not transform or map response models (ts-aggregations-003)

## Defaults
- When all orchestrations are independent, fan out in parallel using `Task.WhenAll`.
- When orchestrations have ordering dependencies, execute sequentially and pass results forward.
