# The Standard Aggregation Services — Rules

## DESIGN

**ts-aggregations-001** [ERROR] Aggregation services must fan out calls to multiple orchestration services in sequence or in parallel.
**ts-aggregations-002** [ERROR] Aggregation services must expose a single entry-point method that triggers the aggregated workflow.
**ts-aggregations-003** [ERROR] Aggregation services must depend only on orchestration services (or processing services where no orchestration exists).
**ts-aggregations-004** [ERROR] Aggregation services must return results only; they must not perform data transformation or enforce business rules.

## TESTING

**ts-aggregations-005** [ERROR] Every behavior must have a failing test written before the implementation (TDD).
**ts-aggregations-006** [ERROR] All test inputs and outputs must be randomized; expected variables must use DeepClone().
