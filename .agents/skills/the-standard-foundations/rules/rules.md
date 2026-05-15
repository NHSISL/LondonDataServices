# The Standard Foundations — Rules

## BUSINESS LANGUAGE

**ts-foundations-001** [ERROR] Foundation service methods must use business language: Add, Retrieve, RetrieveAll, Modify, Remove — never Insert, Select, Update, Delete.

## RESPONSIBILITY

**ts-foundations-002** [ERROR] Foundation services must expose only CRUD operations; business-workflow methods (e.g., Upsert, Process) must not appear.
**ts-foundations-005** [ERROR] Foundation services must integrate with exactly one entity broker.

## VALIDATION

**ts-foundations-003** [ERROR] Foundation services must perform structural validation (null checks, empty Guid, whitespace strings) on all inputs before calling the broker.
**ts-foundations-004** [ERROR] Foundation services must perform logical validation (date range, referential rules) where applicable.

## EXCEPTION HANDLING

**ts-foundations-006** [ERROR] All broker exceptions must be caught and re-thrown as service-specific dependency exception models; unexpected exceptions must be wrapped in service exception models.

## TESTING

**ts-foundations-007** [ERROR] Every behavior must have a failing test written before the implementation (TDD).
**ts-foundations-008** [ERROR] All test inputs and outputs must be randomized; expected variables must use `DeepClone()` to dereference from inputs.
