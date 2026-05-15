---
applyTo: "**/*.cs"
---

# The Standard C# — Cancellation Patterns

## Applies To
Any C# file where `CancellationToken`, timeout logic, or `TryCatch` is used — services, brokers, orchestrations.

## Rules — Do
- `CancellationToken` must be the last parameter in every method signature (tsc-csharp-cp-001)
- `CancellationToken` must default to `default` — never omit the default value (tsc-csharp-cp-002)
- The parameter must be named `cancellationToken` — abbreviated names (`ct`, `token`, `cancelToken`) are forbidden (tsc-csharp-cp-004)
- Call `cancellationToken.ThrowIfCancellationRequested()` before dependency or long-running operations (tsc-csharp-cp-017)
- The same token must flow through every layer — never silently dropped (tsc-csharp-cp-007)
- When timeout is required, link tokens using `CancellationTokenSource.CreateLinkedTokenSource` (tsc-csharp-cp-009)
- When a timeout source exists, `catch (OperationCanceledException) when (timeoutSource.IsCancellationRequested)` must precede the plain `catch (OperationCanceledException)` (tsc-csharp-cp-010)
- A `TimeoutException` from a timeout-triggered cancellation must be wrapped as a dependency failure (tsc-csharp-cp-011)
- Pass the same `CancellationToken` to every parallel task in `Task.WhenAll` (tsc-csharp-cp-014)
- Dependency exception tests must include `TimeoutException` in `Theory` MemberData (tsc-csharp-cp-015)
- Tests must verify `OperationCanceledException` propagates unwrapped (tsc-csharp-cp-016)

## Rules — Do Not
- Must not declare `CancellationToken` as nullable (`CancellationToken?` is forbidden) (tsc-csharp-cp-003)
- Must not add `CancellationToken` to trivial in-memory methods that perform no I/O (tsc-csharp-cp-006)
- Must not create a new `CancellationTokenSource` to replace an upstream token (tsc-csharp-cp-008)
- Must never wrap `OperationCanceledException` in any other exception type — always rethrow with `throw;` (tsc-csharp-cp-012)
- Must not add `catch (OperationCanceledException) { throw; }` when no timeout differentiation exists (tsc-csharp-cp-013)

## Defaults
- When no timeout exists: propagate the token as-is and do not catch `OperationCanceledException`.
- When a timeout is required: use `CreateLinkedTokenSource` and always add both catch blocks in the mandatory order.
