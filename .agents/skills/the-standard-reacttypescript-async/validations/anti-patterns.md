# The Standard React + TypeScript + Vite — Async and Cancellation — Anti-Patterns

## Parallel Dependent Operations

**Violates:** tsr-async-004
**What happens:** `Promise.all([getUser(), getPermissions(userId)])` where `getPermissions` requires the `userId` from `getUser`.
**Why it's wrong:** If `getUser` fails, `getPermissions` may receive `undefined` as its argument, producing a misleading secondary error.
**Fix:** Run sequentially: `const user = await getUser(); const permissions = await getPermissions(user.id);`

## Ignored Rejection

**Violates:** tsr-async-005
**What happens:** `fetchData().then(setData)` — no `.catch` is attached.
**Why it's wrong:** A rejection propagates silently as an unhandled promise rejection. The user sees no error; the developer has no signal.
**Fix:** Add `.catch(setError)` or wrap in `async/await` with `try/catch`.

## Missing Stale Protection

**Violates:** tsr-async-002
**What happens:** A `useEffect` sets state after the component unmounts because it has no mounted flag or abort signal.
**Why it's wrong:** React warns about state updates on unmounted components. In concurrent mode, this can produce subtle bugs.
**Fix:** Add `let isMounted = true;` before the async call, check `if (isMounted)` before `setState`, and return `() => { isMounted = false; }` as cleanup.

## Missing Failure State

**Violates:** tsr-async-001
**What happens:** A page hook has `isLoading` and data state but no `error` state. When the request fails the page renders as if loading forever.
**Why it's wrong:** The user has no feedback that something went wrong and no path to recover.
**Fix:** Add `const [error, setError] = useState<unknown>(null);` and render `<ErrorSummary error={error} />` in the page when error is set.
