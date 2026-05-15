---
applyTo: "**/*Exception*.cs"
---

# The Standard — Exceptions

## Applies To
All exception model definitions and exception handling code across all layers.

## Rules — Do
- Define a three-category exception hierarchy per service: Validation, Dependency, Service (ts-exceptions-001)
- Wrap all broker/infrastructure exceptions as `{Entity}DependencyException` (ts-exceptions-002)
- Wrap all unexpected exceptions as `{Entity}ServiceException` (ts-exceptions-003)
- Wrap all validation failures as `{Entity}ValidationException` (ts-exceptions-004)
- Use `Xeptions` or equivalent as the base class for all custom exceptions (ts-exceptions-005)
- Pass the inner (original) exception as `innerException` to all wrapping exceptions (ts-exceptions-006)
- Include the entity name in every exception class name (ts-exceptions-007)

## Rules — Do Not
- Must not let infrastructure exceptions (`SqlException`, `HttpRequestException`) propagate beyond the foundation service (ts-exceptions-001)
- Must not create generic exceptions without an entity qualifier — `AppException`, `ServiceException` are forbidden (ts-exceptions-002)
- Must not throw the raw `Exception` base class (ts-exceptions-003)
- Must not catch-all with an empty handler that swallows exceptions silently (ts-exceptions-004)

## Defaults
- Dependency validation exceptions (e.g., duplicate key) → `{Entity}DependencyValidationException`.
- Not-found after a retrieval → `NotFound{Entity}Exception` wrapped in `{Entity}ValidationException`.
