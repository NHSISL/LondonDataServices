---
name: the-standard-cancellation-patterns
version: 0.1.0
csharp-standard-version: v0.8
applies-to: ["*.cs"]
depends-on: ["the-standard-csharp-methods", "the-standard-csharp-classes"]
---

# The Standard C# — Cancellation Patterns

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: Any C# file where `CancellationToken`, timeout logic, or `TryCatch` is used.
0.1/ Who: Any engineer writing or reviewing async C# methods that accept `CancellationToken`.
0.2/ What: Governs `CancellationToken` method signatures, token propagation, `TryCatch`
           exception handling for `OperationCanceledException` and `TimeoutException`,
           parallel orchestration, and testing of cancellation behaviour.
0.3/ Applies to: `*.cs` — specifically service, broker, and orchestration files using `TryCatch`.
0.4/ Version: The Standard C# Cancellation Patterns v1.0
0.5/ Depends on: the-standard-csharp-methods, the-standard-csharp-classes

---

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

### 1.0/ Dos

| Rule ID | Requirement |
|---|---|
| [tsc-csharp-cp-001](rules/rules.md#tsc-csharp-cp-001) | `CancellationToken` must be the last parameter in every method signature. |
| [tsc-csharp-cp-002](rules/rules.md#tsc-csharp-cp-002) | `CancellationToken` must default to `default`. |
| [tsc-csharp-cp-004](rules/rules.md#tsc-csharp-cp-004) | Parameter must be named `cancellationToken`. |
| [tsc-csharp-cp-007](rules/rules.md#tsc-csharp-cp-007) | The same token must flow through all layers — never silently dropped. |
| [tsc-csharp-cp-009](rules/rules.md#tsc-csharp-cp-009) | Link tokens for timeout using `CancellationTokenSource.CreateLinkedTokenSource`. |
| [tsc-csharp-cp-017](rules/rules.md#tsc-csharp-cp-017) | Call `cancellationToken.ThrowIfCancellationRequested()` near the start of every method accepting a `CancellationToken`, before any dependency or long-running operation. |
| [tsc-csharp-cp-010](rules/rules.md#tsc-csharp-cp-010)
| [tsc-csharp-cp-014](rules/rules.md#tsc-csharp-cp-014) | Pass the same token to all parallel tasks. |
| [tsc-csharp-cp-015](rules/rules.md#tsc-csharp-cp-015) | `TimeoutException` must appear in Theory MemberData for dependency exception tests. |
| [tsc-csharp-cp-016](rules/rules.md#tsc-csharp-cp-016) | Tests must verify `OperationCanceledException` is never wrapped. |

### 1.1/ Don'ts

| Rule ID | Prohibition |
|---|---|
| [tsc-csharp-cp-003](validations/anti-patterns.md#nullable-cancellationtoken) | `CancellationToken` must not be nullable (`CancellationToken?`). |
| [tsc-csharp-cp-006](validations/anti-patterns.md#token-on-trivial-methods) | Must not add `CancellationToken` to trivial in-memory methods. |
| [tsc-csharp-cp-008](validations/anti-patterns.md#unnecessary-cancellationtokensource) | Must not create a new `CancellationTokenSource` when an upstream token already exists. |
| [tsc-csharp-cp-012](validations/anti-patterns.md#wrapped-operationcanceledexception) | `OperationCanceledException` must never be wrapped in a service or dependency exception. |
| [tsc-csharp-cp-013](validations/anti-patterns.md#unnecessary-catch-block) | When no timeout exists, must not add `catch (OperationCanceledException) { throw; }`. |

### 1.2/ Ask

- Ask when the method uses a timeout but it is unclear which `CancellationTokenSource` owns it.
- Ask when a method at a higher orchestration layer aggregates multiple tokens.
- Ask before adding `CancellationToken` to methods that appear purely in-memory.

### 1.3/ Defaults

- When no timeout exists: propagate the token as-is and do not catch `OperationCanceledException`.
- When a timeout is required: always use `CancellationTokenSource.CreateLinkedTokenSource` and always add both catch blocks in the mandatory order.
- When in doubt whether a method is async I/O: apply `CancellationToken` — omission is a harder bug to find later.

### 1.4/ Examples

| | File |
|---|---|
| ✅ | [examples/good/example_good_method_signature.cs](examples/good/example_good_method_signature.cs) |
| ✅ | [examples/good/example_good_trycatch_with_timeout.cs](examples/good/example_good_trycatch_with_timeout.cs) |
| ✅ | [examples/good/example_good_trycatch_no_timeout.cs](examples/good/example_good_trycatch_no_timeout.cs) |
| ✅ | [examples/good/example_good_token_propagation.cs](examples/good/example_good_token_propagation.cs) |
| ✅ | [examples/good/example_good_parallel_orchestration.cs](examples/good/example_good_parallel_orchestration.cs) |
| ✅ | [examples/good/example_good_test_cancellation.cs](examples/good/example_good_test_cancellation.cs) |
| ❌ | [examples/bad/example_bad_nullable_token.cs](examples/bad/example_bad_nullable_token.cs) |
| ❌ | [examples/bad/example_bad_abbreviated_name.cs](examples/bad/example_bad_abbreviated_name.cs) |
| ❌ | [examples/bad/example_bad_dropped_token.cs](examples/bad/example_bad_dropped_token.cs) |
| ❌ | [examples/bad/example_bad_wrapped_cancellation.cs](examples/bad/example_bad_wrapped_cancellation.cs) |
| ❌ | [examples/bad/example_bad_catch_order.cs](examples/bad/example_bad_catch_order.cs) |
| ❌ | [examples/bad/example_bad_unnecessary_catch.cs](examples/bad/example_bad_unnecessary_catch.cs) |
| ❌ | [examples/bad/example_bad_missing_throw_if_cancelled.cs](examples/bad/example_bad_missing_throw_if_cancelled.cs) |

---

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# methods with correct `CancellationToken` signatures and `TryCatch` patterns.
2.1/ Outcome: All cancellation and timeout handling is correct, propagated, and testable.
2.2/ Tone: Direct. Cite rule IDs (e.g., `tsc-csharp-cp-010`). No personal preference.
