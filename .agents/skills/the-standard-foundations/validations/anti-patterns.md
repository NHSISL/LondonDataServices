# The Standard Foundations — Anti-Patterns

## Multi-Broker Foundation

**Violates:** ts-foundations-005
**What happens:** A `StudentService` injects both `IStorageBroker` and `IEmailBroker` to send a welcome email after adding a student.
**Why it's wrong:** Foundation services must be single-entity and single-broker. Cross-broker coordination belongs in an Orchestration service.
**Fix:** Create a `StudentOrchestrationService` that calls `StudentService.AddStudentAsync` and `NotificationService.SendWelcomeEmailAsync`.

## Workflow in Foundation

**Violates:** ts-foundations-002
**What happens:** `StudentService` exposes `UpsertStudentAsync` which internally calls `AddStudentAsync` or `ModifyStudentAsync` based on existence.
**Why it's wrong:** Upsert is a business workflow, not a CRUD operation. Foundation services must not contain conditional routing logic.
**Fix:** Move `UpsertStudentAsync` to a `StudentProcessingService`.

## Naked Broker Exceptions

**Violates:** ts-foundations-006
**What happens:** A `SqlException` from the broker propagates uncaught through the service to the controller.
**Why it's wrong:** Consumers must not be aware of storage technology details. Infrastructure exceptions must be wrapped in domain exception models.
**Fix:** Catch `SqlException` (via EFxceptions) and re-throw as `StudentDependencyException`.

## Hardcoded Test Values

**Violates:** ts-foundations-008
**What happens:** A test always passes student `Id = Guid.Parse("00000000-...")` and expects a fixed Name "Hassan".
**Why it's wrong:** Hardcoded values allow implementations to pass tests by returning constants rather than implementing real logic.
**Fix:** Use `CreateRandomStudent()` helper to generate a `Filler<Student>`-populated instance and `DeepClone()` for the expected variable.
