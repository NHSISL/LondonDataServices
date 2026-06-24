# The Standard Orchestration Services — Rules

## DESIGN

**ts-orchestrations-001** [ERROR] Orchestration services must coordinate two or more foundation or processing services to fulfil a multi-entity business requirement.
**ts-orchestrations-002** [ERROR] Orchestration services must expose named business-operation methods reflecting the multi-entity workflow.
**ts-orchestrations-003** [ERROR] Orchestration services must depend only on foundation services, processing services, or other orchestration services — never on brokers.
**ts-orchestrations-004** [ERROR] Cross-entity validation and business rules must be handled at the orchestration layer, not pushed down into foundations.

## TESTING

**ts-orchestrations-005** [ERROR] Every behavior must have a failing test written before the implementation (TDD).
**ts-orchestrations-006** [ERROR] All test inputs and outputs must be randomized; expected variables must use DeepClone().
