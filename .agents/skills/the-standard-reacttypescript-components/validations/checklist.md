# The Standard React + TypeScript + Vite — Components — Checklist

- [ ] Component renders UI from props — no data fetching inside the component body (tsr-components-001, tsr-components-002)
- [ ] No broker or `fetch()` calls inside the component (tsr-components-003)
- [ ] No foundation service calls inside the component (tsr-components-004)
- [ ] No business-rule ternaries or conditionals in JSX (tsr-components-005)
- [ ] Component is small and focused — no mixed loading + transformation + rendering (tsr-components-006)
- [ ] Prop model is declared as a `type` for non-trivial components (tsr-components-007)
- [ ] Component does not duplicate state present in parent, hook, or view model (tsr-components-008)
- [ ] All interactive elements use semantic HTML (`<button>`, `<a>`, `<input>`) (tsr-components-009)
- [ ] No `<div onClick={...}>` used as a button substitute (tsr-components-009)
