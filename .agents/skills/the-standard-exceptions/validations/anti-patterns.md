# The Standard Exceptions — Anti-Patterns

## Naked Infrastructure Exception

**Violates:** ts-exceptions-002
**What happens:** A `SqlException` thrown by EF Core propagates out of the foundation service, through the controller, and reaches the client as a 500 with a SQL error message.
**Why it's wrong:** Infrastructure exception details are leaked to consumers, coupling them to the storage technology and exposing internal system details.
**Fix:** Catch the `SqlException` (via EFxceptions pattern) in the service's `TryCatch` block and re-throw as `StudentDependencyException(sqlException)`.

## Generic Exception Name

**Violates:** ts-exceptions-007
**What happens:** A service throws `new ValidationException("Student name is required.")` with no entity qualifier.
**Why it's wrong:** Generic names collide across services and make exception filtering at the exposer layer ambiguous.
**Fix:** Define and throw `new StudentValidationException(new InvalidStudentException())` where `InvalidStudentException` carries the validation message.

## Raw Exception Throw

**Violates:** ts-exceptions-005
**What happens:** Code throws `throw new Exception("Something went wrong.")`.
**Why it's wrong:** The base `Exception` type carries no domain context, cannot be filtered by type, and provides no actionable information to consumers.
**Fix:** Define `FailedServiceStudentException : Xeption` and throw it wrapped in `StudentServiceException`.

## Swallowed Exception

**Violates:** ts-exceptions-002, ts-exceptions-003
**What happens:** A `catch (Exception) { }` block with an empty body discards the exception silently.
**Why it's wrong:** Silent exception swallowing hides failures, making the system appear healthy while data is corrupted or operations are incomplete.
**Fix:** Always re-throw as a wrapped domain exception or at minimum log via the logging broker before re-throwing.
