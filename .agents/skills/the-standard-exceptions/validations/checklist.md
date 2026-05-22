# The Standard Exceptions — Checklist

- [ ] ts-exceptions-001: Service defines ValidationException, DependencyException, and ServiceException for the entity.
- [ ] ts-exceptions-002: All broker/infrastructure exceptions are caught and wrapped as DependencyException.
- [ ] ts-exceptions-003: All unexpected exceptions are caught and wrapped as ServiceException.
- [ ] ts-exceptions-004: All validation failures are thrown as ValidationException with specific inner exception.
- [ ] ts-exceptions-005: All custom exception classes inherit from Xeption.
- [ ] ts-exceptions-006: All wrapping exceptions pass the original as innerException.
- [ ] ts-exceptions-007: Every exception class name includes the entity name.
- [ ] ts-exceptions-008: Infrastructure validation failures (duplicate key, FK violation) wrapped as DependencyValidationException.
- [ ] ts-exceptions-009: Not-found conditions throw NotFound{Entity}Exception wrapped in {Entity}ValidationException.
