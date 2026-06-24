# The Standard React + TypeScript + Vite — Pages — Anti-Patterns

## Broker in Page

**Violates:** tsr-pages-003
**What happens:** A page calls `fetch("/api/dashboard")` inside a `useEffect`.
**Why it's wrong:** Pages are exposure-layer route entry points. Infrastructure calls bypass the service layers and make the page untestable.
**Fix:** Move the fetch into a view service. Call the view service from the page hook. Render from hook state.

## Business Rule in Page

**Violates:** tsr-pages-004
**What happens:** A page filters data with `const adults = patients.filter(p => p.age >= 18)` before rendering.
**Why it's wrong:** Age-based filtering is a business rule that belongs in a foundation or view service. Placing it in the page duplicates logic and hides it from tests.
**Fix:** Return `activePatientCount` from the view service. The page only renders what the hook provides.

## Missing States

**Violates:** tsr-pages-005
**What happens:** A page renders the data component immediately without checking `isLoading` or `error`.
**Why it's wrong:** When data is loading or fails, the page renders broken or empty UI silently.
**Fix:** Add explicit loading (`<LoadingIndicator />`), error (`<ErrorSummary />`), and empty (`<EmptyState />`) branches.
