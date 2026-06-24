# The Standard Exposers — Anti-Patterns

## Controller with Logic

**Violates:** ts-exposers-006
**What happens:** The POST action validates the student, sets the creation date, and checks for duplicates before calling the service.
**Why it's wrong:** Controllers are exposers only. Business logic belongs in the service layer.
**Fix:** Move all validation and computation into the foundation or processing service. The controller calls the service and maps the result to an HTTP response.

## Controller Broker Access

**Violates:** ts-exposers-005
**What happens:** `StudentsController` injects `IStorageBroker` to do a quick lookup before calling the service.
**Why it's wrong:** Controllers must be technology-agnostic. Storage access must go through services.
**Fix:** Add the required query to the service interface and call it from the controller.

## Exception Detail Leak

**Violates:** ts-exposers-004
**What happens:** The controller returns `StatusCode(500, exception.ToString())` exposing the stack trace.
**Why it's wrong:** Internal exception details reveal implementation specifics and are a security risk.
**Fix:** Return `Problem()` or a generic error response; log the full exception via the logging broker in the service layer.

## Singular Controller Name

**Violates:** ts-exposers-001
**What happens:** The class is named `StudentController` exposing `api/student`.
**Why it's wrong:** REST resources are plural by convention. The route and class name must both be plural.
**Fix:** Rename to `StudentsController`; the route becomes `api/students`.
