---
applyTo: "**/*OrchestrationService*.cs"
---

# The Standard — Orchestration Services

## Applies To
The coordination layer above foundation and processing services — `*OrchestrationService*.cs`, `*OrchestrationServiceTests*.cs`.

## Rules — Do
- Coordinate two or more foundation or processing services to fulfil a multi-entity business requirement (ts-orchestrations-001)
- Expose named business-operation methods that reflect the multi-entity workflow (ts-orchestrations-002)
- Depend only on foundation services, processing services, or other orchestration services (ts-orchestrations-003)
- Handle cross-entity validation and business rules at this layer (ts-orchestrations-004)
- Write a failing test before every behavior (ts-orchestrations-005)

## Rules — Do Not
- Must not call brokers directly (ts-orchestrations-001)
- Must not duplicate validation already enforced by the underlying services (ts-orchestrations-002)
- Must not manage transactions or unit-of-work patterns directly (ts-orchestrations-003)

## Defaults
- When coordination involves an existence check across entities, delegate each entity check to its own foundation or processing service.
- When coordination spans more than three entity types, consider a Management or Aggregation service.
