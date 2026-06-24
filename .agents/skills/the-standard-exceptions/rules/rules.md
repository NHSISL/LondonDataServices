# The Standard Exceptions — Rules

## HIERARCHY

**ts-exceptions-001** [ERROR] Each service must define a three-category exception hierarchy: `{Entity}ValidationException`, `{Entity}DependencyException`, `{Entity}ServiceException`.
**ts-exceptions-005** [ERROR] All custom exception classes must inherit from `Xeption` (or the project's designated base exception class).
**ts-exceptions-007** [ERROR] Every exception class name must include the entity name (e.g., `StudentValidationException`, not `ValidationException`).

## WRAPPING

**ts-exceptions-002** [ERROR] All broker/infrastructure exceptions must be caught and re-thrown wrapped as `{Entity}DependencyException`.
**ts-exceptions-003** [ERROR] All unexpected exceptions must be caught and re-thrown wrapped as `{Entity}ServiceException`.
**ts-exceptions-004** [ERROR] All validation failures must be thrown as `{Entity}ValidationException` with the specific validation error as the inner exception.
**ts-exceptions-006** [ERROR] All wrapping exceptions must receive the original exception as the `innerException` constructor argument.

## DEPENDENCY VALIDATION

**ts-exceptions-008** [ERROR] Infrastructure-reported validation failures (e.g., duplicate key, foreign key violation) must be wrapped as `{Entity}DependencyValidationException`, not `{Entity}DependencyException`.

## NOT FOUND

**ts-exceptions-009** [ERROR] A not-found condition after a retrieval must throw `NotFound{Entity}Exception` wrapped inside `{Entity}ValidationException`.
