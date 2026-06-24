# The Standard React + TypeScript + Vite — Hooks — Checklist

- [ ] Hook manages React lifecycle and state only — no business rules (tsr-hooks-001, tsr-hooks-003)
- [ ] Hook does not fetch data and apply domain filters inline (tsr-hooks-002)
- [ ] Page hook calls a view service injected as a parameter (tsr-hooks-004)
- [ ] All async effects use a mounted flag or AbortController with cleanup (tsr-hooks-005)
- [ ] No `useEffect` used for data transformation that belongs in a view service (tsr-hooks-006)
- [ ] No suppressed ESLint hook dependency warnings without a documented reason (tsr-hooks-007)
- [ ] Hook file is named `use{Purpose}.ts` and placed in `src/hooks/{pages|components}/`
