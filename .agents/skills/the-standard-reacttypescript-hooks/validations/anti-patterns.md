# The Standard React + TypeScript + Vite — Hooks — Anti-Patterns

## Hook Replacing Service

**Violates:** tsr-hooks-002, tsr-hooks-003
**What happens:** A hook fetches patients, filters by age, and returns the result — acting as a combined broker + service:
```ts
export function usePatients() {
    async function retrievePatientsAsync() {
        const response = await fetch("/api/patients");
        const patients = await response.json();
        return patients.filter((p: any) => p.age >= 18);
    }
    return { retrievePatientsAsync };
}
```
**Why it's wrong:** Domain filtering (`age >= 18`) is a business rule. Broker calls (`fetch`) belong in brokers. Both belong in services, not hooks.
**Fix:** Move the fetch to `PatientApiBroker`, the age filter to `PatientService`, and the view model shaping to `PatientViewService`. The hook calls the view service and manages state.

## No Stale Update Protection

**Violates:** tsr-hooks-005
**What happens:** An async effect sets state after the component has unmounted:
```ts
useEffect(() => {
    async function load() {
        const data = await viewService.retrieveAsync();
        setData(data); // ❌ may run after unmount
    }
    load();
}, []);
```
**Why it's wrong:** Setting state on an unmounted component causes React warnings and potential memory leaks.
**Fix:** Add a mounted flag and check it before calling `setData`:
```ts
useEffect(() => {
    let isMounted = true;
    async function load() {
        const data = await viewService.retrieveAsync();
        if (isMounted) setData(data);
    }
    load();
    return () => { isMounted = false; };
}, [viewService]);
```

## useEffect for Transformation

**Violates:** tsr-hooks-006
**What happens:** A `useEffect` watches `patients` and derives `adultCount` into a separate state variable.
**Why it's wrong:** Derived values that depend only on existing state should be computed during render, not in effects.
**Fix:** Compute `const adultCount = patients.filter(p => p.isAdult).length;` directly in the render path, or return `adultCount` from the view service.
