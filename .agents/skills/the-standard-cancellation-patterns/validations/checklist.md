---
skill: the-standard-cancellation-patterns
type: checklist
source-section: "3.0–9.0"
---

# The Standard C# — Cancellation Patterns — Checklist

Use this checklist before completing any work that touches `CancellationToken`, timeout logic, or `TryCatch` blocks.

---

## Method Signature

- [ ] `CancellationToken` is the last parameter in the method signature (tsc-csharp-cp-001)
- [ ] `CancellationToken` defaults to `default` (tsc-csharp-cp-002)
- [ ] `CancellationToken` is not nullable — no `CancellationToken?` (tsc-csharp-cp-003)
- [ ] Parameter is named `cancellationToken` — not `ct`, `token`, or `cancelToken` (tsc-csharp-cp-004)

## Applicability

- [ ] `CancellationToken` is only present on methods that perform I/O or long-running work (tsc-csharp-cp-005, tsc-csharp-cp-006)

## Token Propagation

- [ ] Token is propagated through all layers — never silently dropped (tsc-csharp-cp-007)
- [ ] No new `CancellationTokenSource` created unnecessarily to replace an upstream token (tsc-csharp-cp-008)
- [ ] When timeout is required: `CancellationTokenSource.CreateLinkedTokenSource` is used to link the upstream token with the timeout source (tsc-csharp-cp-009)
- [ ] `cancellationToken.ThrowIfCancellationRequested()` is called near the start of the method, before dependency or long-running operations (tsc-csharp-cp-017)

## TryCatch Exception Handling

- [ ] When timeout exists: `catch (OperationCanceledException) when (timeoutSource.IsCancellationRequested)` precedes the plain `catch (OperationCanceledException)` (tsc-csharp-cp-010)
- [ ] When timeout fires: a `TimeoutException` is created and wrapped as a dependency failure (tsc-csharp-cp-011)
- [ ] `OperationCanceledException` is rethrown with `throw;` — never wrapped in another exception (tsc-csharp-cp-012)
- [ ] When no timeout exists: no `catch (OperationCanceledException)` block is present — the exception propagates naturally (tsc-csharp-cp-013)

## Parallel Orchestration

- [ ] All parallel tasks in a `Task.WhenAll` fan-out receive the same `CancellationToken` (tsc-csharp-cp-014)

## Testing

- [ ] Dependency exception `Theory` MemberData includes `TimeoutException` (tsc-csharp-cp-015)
- [ ] A test verifies `OperationCanceledException` propagates unwrapped — not as `DependencyException` or any other type (tsc-csharp-cp-016)
