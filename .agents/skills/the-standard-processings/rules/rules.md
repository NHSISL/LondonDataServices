# The Standard Processing Services — Rules

## DESIGN

**ts-processings-001** [ERROR] Processing services must combine two or more foundation service primitives to form a business workflow.
**ts-processings-002** [ERROR] Processing services must expose higher-order business verbs (e.g., Ensure, Process, Upsert, Calculate), not raw CRUD verbs.
**ts-processings-003** [ERROR] Processing services must depend only on foundation services (or other processing services); they must never depend on brokers directly.
**ts-processings-004** [ERROR] Cross-cutting workflow concerns (e.g., existence checks, conditional writes) must be handled inside the processing service.

## TESTING

**ts-processings-005** [ERROR] Every behavior must have a failing test written before the implementation (TDD).
**ts-processings-006** [ERROR] All test inputs and outputs must be randomized; expected variables must use DeepClone() to dereference from inputs.
