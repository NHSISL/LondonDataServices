# The Standard Orchestration Services — Anti-Patterns

## Broker Bypass

**Violates:** ts-orchestrations-003
**What happens:** `StudentLibraryOrchestrationService` injects `IStorageBroker` to check whether a library card exists before enrolling a student.
**Why it's wrong:** All storage access must go through a foundation service. Direct broker access skips validation and exception handling.
**Fix:** Inject `ILibraryCardService` and call `RetrieveLibraryCardByStudentIdAsync`.

## Duplicate Validation

**Violates:** ts-orchestrations-004
**What happens:** The orchestration service re-validates that the student Id is not empty, which the underlying `StudentService` already validates.
**Why it's wrong:** Validation duplication inflates test counts and obscures the authoritative validation location.
**Fix:** Trust the foundation/processing service validation. The orchestration service validates only cross-entity rules that the lower services cannot enforce.

## Direct Transaction Management

**Violates:** ts-orchestrations-003
**What happens:** The orchestration service manually opens a `DbTransaction`, calls multiple services, then commits or rolls back.
**Why it's wrong:** Transaction management is an infrastructure concern that belongs in the broker layer. Orchestration services must not depend on infrastructure types.
**Fix:** Use an outbox pattern or a saga-based approach mediated through brokers and foundation services.
