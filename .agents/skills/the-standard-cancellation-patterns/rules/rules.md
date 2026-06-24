---
skill: the-standard-cancellation-patterns
type: rules
source-section: "3.0–9.0"
---

# The Standard C# — Cancellation Patterns — Rules

Severity levels: `[ERROR]` = must fix · `[WARN]` = should fix.

---

## Method Signature Conventions (source: section 3)

**tsc-csharp-cp-001** [ERROR] `CancellationToken` must be the last parameter in every method signature.

**tsc-csharp-cp-002** [ERROR] `CancellationToken` must default to `default` — never omit the default value.

**tsc-csharp-cp-003** [ERROR] `CancellationToken` must not be nullable (`CancellationToken?` is forbidden).

**tsc-csharp-cp-004** [ERROR] The parameter must be named `cancellationToken` — abbreviated names (`ct`, `token`, `cancelToken`) are forbidden.

---

## Applicability (source: sections 4–5)

**tsc-csharp-cp-005** [WARN] `CancellationToken` should be accepted by methods that perform database operations, HTTP operations, file operations, queue/messaging operations, long-running work, orchestration fan-out, or parallel operations.

**tsc-csharp-cp-006** [WARN] `CancellationToken` must not be added to trivial in-memory methods that perform no I/O and have no waiting behaviour.

---

## Token Propagation (source: section 6)

**tsc-csharp-cp-007** [ERROR] The same token must flow through every layer: Controller → Coordination → Orchestration → Foundation → Broker → Dependency. The token must never be silently dropped.

**tsc-csharp-cp-008** [ERROR] A new `CancellationTokenSource` must not be created to replace an upstream token. Use `CreateLinkedTokenSource` when a timeout must be added.

**tsc-csharp-cp-009** [ERROR] When timeout behaviour is required, tokens must be linked using `CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutSource.Token)`.

**tsc-csharp-cp-017** [ERROR] Any method that accepts a `CancellationToken` must call `cancellationToken.ThrowIfCancellationRequested()` before executing dependency or long-running operations. This ensures fail-fast behaviour and prevents unnecessary work from continuing after cancellation has already been requested.

---

## TryCatch Exception Handling

**tsc-csharp-cp-010** [ERROR] When a timeout source exists, the guarded catch block `catch (OperationCanceledException) when (timeoutSource.IsCancellationRequested)` must always appear before the plain `catch (OperationCanceledException)` block. This ordering is mandatory.

**tsc-csharp-cp-011** [ERROR] A `TimeoutException` (produced from a timeout-triggered `OperationCanceledException`) must be wrapped and treated as a dependency failure (e.g., passed to `CreateAndLogDependencyException`).

**tsc-csharp-cp-012** [ERROR] `OperationCanceledException` must never be wrapped in any other exception type. It must always be rethrown with `throw;`.

**tsc-csharp-cp-013** [ERROR] When a method uses `CancellationToken` but has no timeout differentiation, `catch (OperationCanceledException)` must not be present — the exception propagates naturally.

---

## Parallel Orchestration (source: section 8)

**tsc-csharp-cp-014** [ERROR] The same `CancellationToken` must be passed to every parallel task. Each call in a `Task.WhenAll` fan-out must receive the token.

---

## Testing (source: section 9)

**tsc-csharp-cp-015** [ERROR] Dependency exception tests must include `TimeoutException` in the `Theory` `MemberData` to validate localization and categorization.

**tsc-csharp-cp-016** [ERROR] Tests must verify that a true cancellation results in `OperationCanceledException` propagating unwrapped — never as `DependencyException` or any other wrapper type.
