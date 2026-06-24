# The Standard React + TypeScript + Vite — State Management — Checklist

- [ ] State is as local as possible — elevated only when siblings genuinely share it (tsr-state-001)
- [ ] Server data is owned by the page hook — not duplicated in child component state (tsr-state-002)
- [ ] Shareable/bookmarkable state uses URL params or path segments (tsr-state-003)
- [ ] Local UI state (modal, dropdown, pre-submit input) uses component `useState` (tsr-state-004)
- [ ] Application state is used only for authenticated user, theme, feature flags, or tenant context (tsr-state-005)
- [ ] No global state added to avoid designing props (tsr-state-006)
- [ ] Business-meaningful state transitions (submit, approve, publish) call a service before setting state (tsr-state-007)
