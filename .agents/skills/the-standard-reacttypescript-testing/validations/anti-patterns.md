# The Standard React + TypeScript + Vite — Testing — Anti-Patterns

## Testing React Internals

**Violates:** tsr-testing-006
**What happens:** A test asserts `expect(component.state.isLoading).toBe(false)` or verifies that `useState` was called twice.
**Why it's wrong:** React internals are implementation details. Refactoring state management (e.g., from `useState` to a reducer) should not break tests.
**Fix:** Assert on what the user sees: `expect(screen.queryByRole("progressbar")).not.toBeInTheDocument()`.

## Large Snapshot as Sole Proof

**Violates:** tsr-testing-007
**What happens:** A test renders a full page and calls `expect(container).toMatchSnapshot()` as its only assertion.
**Why it's wrong:** Any layout or text change breaks the snapshot. The test does not communicate what the correct behavior is.
**Fix:** Add targeted assertions on specific visible elements. Keep snapshots small and supplementary.

## Mock Inside Service Layer

**Violates:** tsr-testing-008
**What happens:** A foundation service test mocks a private helper method inside the service.
**Why it's wrong:** Mocking internal implementation details couples the test to the implementation. Refactoring the helper breaks tests that should not care about it.
**Fix:** Mock only the broker interface. Test the service through its public method contract.
