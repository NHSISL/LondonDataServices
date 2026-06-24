---
applyTo: "src/hooks/**/*,**/use*.ts"
---

# The Standard React + TypeScript + Vite — Hooks

## Applies To
All custom React hooks in `src/hooks/**/*` and any `use*.ts` co-located with a page or component.

## Rules — Do
- A hook may manage React lifecycle and state (tsr-hooks-001)
- A page hook may call a view service to retrieve view models (tsr-hooks-004)
- Effects that perform async work must protect against stale updates using mounted flags or `AbortController` (tsr-hooks-005)

## Rules — Do Not
- Never use a hook to replace a foundation service (tsr-hooks-002)
- Never put core business rules inside a hook (tsr-hooks-003)
- Never use `useEffect` for data transformation that belongs in a view service (tsr-hooks-006)
- Never suppress hook dependency warnings without a documented reason (tsr-hooks-007)

## Defaults
- A page hook manages state (`isLoading`, `error`, data) and calls the view service inside a `useEffect`.
- A component hook manages local UI state (modal open/closed, form input, dropdown state).
