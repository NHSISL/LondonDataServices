# The Standard React + TypeScript + Vite — Routing — Checklist

- [ ] All routes point to page components — no routes point directly to cards, lists, or forms (tsr-routing-001)
- [ ] All route definitions are centralized in a single routes file (tsr-routing-002)
- [ ] Route guards delegate access decisions to a service — no inline role checks (tsr-routing-003)
- [ ] No `{user.role === "X" && <Route ... />}` patterns in route JSX (tsr-routing-004)
