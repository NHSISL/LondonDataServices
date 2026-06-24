---
skill: the-standard-cancellation-patterns
type: anti-patterns
source-section: "3.0–10.0"
---

# The Standard C# — Cancellation Patterns — Anti-Patterns

---

## Nullable CancellationToken

**Violates:** tsc-csharp-cp-003

**What happens:** The parameter is declared as `CancellationToken?` instead of `CancellationToken`.

**Why it's wrong:** Nullable tokens require null-checks at every call site, break propagation contracts, and signal uncertainty about whether cancellation is supported. `CancellationToken.None` and `default` already represent "no cancellation".

**Fix:** Replace `CancellationToken? cancellationToken` with `CancellationToken cancellationToken = default`.

```csharp
// ❌
public ValueTask<Student> RetrieveStudentByIdAsync(
    Guid studentId,
    CancellationToken? cancellationToken)

// ✅
public ValueTask<Student> RetrieveStudentByIdAsync(
    Guid studentId,
    CancellationToken cancellationToken = default)
```

---

## Abbreviated Parameter Name

**Violates:** tsc-csharp-cp-004

**What happens:** The `CancellationToken` parameter is named `ct`, `token`, or `cancelToken`.

**Why it's wrong:** Abbreviated names reduce readability and make grep/tooling searches unreliable across the codebase. The canonical name is `cancellationToken`.

**Fix:** Rename the parameter to `cancellationToken` at the declaration and all call sites.

```csharp
// ❌
public ValueTask<Student> RetrieveStudentByIdAsync(
    Guid studentId,
    CancellationToken ct = default)

// ✅
public ValueTask<Student> RetrieveStudentByIdAsync(
    Guid studentId,
    CancellationToken cancellationToken = default)
```

---

## Silently Dropped Token

**Violates:** tsc-csharp-cp-007

**What happens:** The `CancellationToken` is accepted by a method but not forwarded to the underlying dependency call.

**Why it's wrong:** The consumer's cancellation signal never reaches the I/O layer, so the operation cannot be interrupted. The token is accepted under false pretence.

**Fix:** Pass `cancellationToken` to every downstream call that accepts it.

```csharp
// ❌
await storageBroker.SelectStudentByIdAsync(studentId);

// ✅
await storageBroker.SelectStudentByIdAsync(
    studentId,
    cancellationToken);
```

---

## Unnecessary New CancellationTokenSource

**Violates:** tsc-csharp-cp-008

**What happens:** A new `CancellationTokenSource` is created and its token replaces the upstream token instead of being linked to it.

**Why it's wrong:** This breaks propagation. The caller's cancellation can no longer reach the dependency. Only the locally-created source can cancel the operation.

**Fix:** If a timeout is needed, use `CancellationTokenSource.CreateLinkedTokenSource` to combine the upstream token with the timeout source.

```csharp
// ❌
var source = new CancellationTokenSource();
await dependency.CallAsync(source.Token);

// ✅
using var timeoutSource =
    new CancellationTokenSource(TimeSpan.FromSeconds(30));

using var linkedSource =
    CancellationTokenSource.CreateLinkedTokenSource(
        cancellationToken,
        timeoutSource.Token);

await dependency.CallAsync(linkedSource.Token);
```

---

## Wrapped OperationCanceledException

**Violates:** tsc-csharp-cp-012

**What happens:** `OperationCanceledException` is caught and rethrown wrapped inside a `FailedServiceException`, `DependencyException`, or any other exception type.

**Why it's wrong:** Wrapping destroys the cooperative cancellation contract. Callers that inspect the exception type (e.g., ASP.NET Core, Polly, hosted services) can no longer detect a true cancellation and will treat it as an error.

**Fix:** Always rethrow with `throw;` — never wrap.

```csharp
// ❌
catch (OperationCanceledException exception)
{
    throw new FailedServiceException(exception);
}

// ❌
catch (OperationCanceledException exception)
{
    throw new DependencyException(exception);
}

// ✅
catch (OperationCanceledException)
{
    throw;
}
```

---

## Wrong Catch Block Ordering

**Violates:** tsc-csharp-cp-010

**What happens:** The plain `catch (OperationCanceledException)` block appears before the guarded `catch (OperationCanceledException) when (timeoutSource.IsCancellationRequested)` block.

**Why it's wrong:** The plain catch is a superset — it matches every `OperationCanceledException`, including those triggered by the timeout source. The guarded catch is therefore unreachable, so timeouts are never classified as dependency failures.

**Fix:** The guarded block must always come first.

```csharp
// ❌
catch (OperationCanceledException)
{
    throw;
}
catch (OperationCanceledException)
    when (timeoutSource.IsCancellationRequested)
{
    throw CreateAndLogDependencyException(new TimeoutException("..."));
}

// ✅
catch (OperationCanceledException)
    when (timeoutSource.IsCancellationRequested)
{
    throw CreateAndLogDependencyException(new TimeoutException("..."));
}
catch (OperationCanceledException)
{
    throw;
}
```

---

## Unnecessary catch (OperationCanceledException) Block

**Violates:** tsc-csharp-cp-013

**What happens:** A `catch (OperationCanceledException) { throw; }` block is added to a `TryCatch` method that uses only a plain `CancellationToken` and has no timeout differentiation.

**Why it's wrong:** The block adds noise, implies timeout logic is present when it is not, and is redundant — `OperationCanceledException` propagates naturally without a catch block.

**Fix:** Remove the catch block entirely when no `timeoutSource` exists.

```csharp
// ❌
public ValueTask<Student> RetrieveStudentAsync(
    Guid studentId,
    CancellationToken cancellationToken = default) =>
TryCatch(async () =>
{
    return await storageBroker.SelectAsync<Student>(
        studentId,
        cancellationToken);

    catch (OperationCanceledException)
    {
        throw;
    }
});

// ✅
public ValueTask<Student> RetrieveStudentAsync(
    Guid studentId,
    CancellationToken cancellationToken = default) =>
TryCatch(async () =>
{
    return await storageBroker.SelectAsync<Student>(
        studentId,
        cancellationToken);
});
```

---

## Missing ThrowIfCancellationRequested

**Violates:** tsc-csharp-cp-017

**What happens:** A method accepts a `CancellationToken` but never calls `cancellationToken.ThrowIfCancellationRequested()` before beginning its work.

**Why it's wrong:** If cancellation has already been requested by the time the method is entered, work proceeds unnecessarily. `ThrowIfCancellationRequested()` provides fail-fast behaviour — it terminates immediately without touching the dependency.

**Fix:** Call `cancellationToken.ThrowIfCancellationRequested()` near the start of the method, inside the `TryCatch` lambda, before any dependency call.

```csharp
// ❌
public ValueTask<Student> RetrieveStudentAsync(
    Guid studentId,
    CancellationToken cancellationToken = default) =>
TryCatch(async () =>
{
    return await storageBroker.SelectAsync<Student>(
        studentId,
        cancellationToken);
});

// ✅
public ValueTask<Student> RetrieveStudentAsync(
    Guid studentId,
    CancellationToken cancellationToken = default) =>
TryCatch(async () =>
{
    cancellationToken.ThrowIfCancellationRequested();

    return await storageBroker.SelectAsync<Student>(
        studentId,
        cancellationToken);
});
```

---

## Missing TimeoutException in Test Theory MemberData

**Violates:** tsc-csharp-cp-015

**What happens:** The `Theory` MemberData for dependency exception tests does not include `TimeoutException`.

**Why it's wrong:** The test matrix is incomplete. Timeout localisation and categorisation paths are never exercised, leaving timeout bugs undetected.

**Fix:** Add `TimeoutException` to the dependency exception MemberData alongside other dependency exceptions.

```csharp
// ❌
public static TheoryData<Exception> DependencyExceptions =>
    new TheoryData<Exception>
    {
        new HttpRequestException(),
        new SqlException()
    };

// ✅
public static TheoryData<Exception> DependencyExceptions =>
    new TheoryData<Exception>
    {
        new HttpRequestException(),
        new SqlException(),
        new TimeoutException()
    };
```
