# The Standard React + TypeScript + Vite — Async and Cancellation — Rules

## State Coverage

**tsr-async-001** [ERROR] Async page loading must explicitly handle all four states: loading, success, empty, and failure. Rendering nothing silently on failure or empty is a violation.

## Stale Update Protection

**tsr-async-002** [ERROR] Effects that perform async work must protect against stale updates after unmount. Acceptable patterns:
- Mounted flag (`let isMounted = true` with cleanup `isMounted = false`)
- `AbortController` with `signal` passed to fetch and cleanup `controller.abort()`
- Framework-approved loaders with built-in cancellation

## Independent Parallelism

**tsr-async-003** [ERROR] Use `Promise.all` only when operations are truly independent — neither result depends on the other's completion or output.

## No Parallel Dependents

**tsr-async-004** [ERROR] Dependent async operations must be executed sequentially. Do not run in parallel with `Promise.all` when the second operation requires the result of the first.

## No Ignored Rejections

**tsr-async-005** [ERROR] Rejected promises must never be ignored. Every `then` chain must have a corresponding `.catch`, and every `async` function in a `useEffect` must be wrapped in `try/catch`.
