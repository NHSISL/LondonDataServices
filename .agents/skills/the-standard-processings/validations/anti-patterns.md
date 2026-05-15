# The Standard Processing Services — Anti-Patterns

## Broker Bypass

**Violates:** ts-processings-003
**What happens:** `StudentProcessingService` injects `IStorageBroker` directly to perform a custom query.
**Why it's wrong:** Processing services must be technology-agnostic. All storage access must go through a foundation service.
**Fix:** Add the required query method to `IStudentService` and call it from the processing service.

## Duplicate Validation

**Violates:** ts-processings-004
**What happens:** `EnsureStudentExistsAsync` repeats null checks and empty-Guid validation already present in the underlying `StudentService.RetrieveStudentByIdAsync`.
**Why it's wrong:** Validation duplication creates maintenance burden and obscures where the authoritative check lives.
**Fix:** Remove structural validation from the processing service; trust the foundation service to enforce it.

## CRUD Exposure

**Violates:** ts-processings-002
**What happens:** `StudentProcessingService` exposes `AddStudentAsync`, `RetrieveStudentByIdAsync`, `ModifyStudentAsync`, `RemoveStudentByIdAsync` as pass-throughs.
**Why it's wrong:** A processing service that only passes through CRUD calls adds no value and violates single-responsibility; consumers should call the foundation service directly.
**Fix:** Remove pass-through CRUD methods. Expose only higher-order workflow methods (e.g., `UpsertStudentAsync`, `EnsureStudentExistsAsync`).
