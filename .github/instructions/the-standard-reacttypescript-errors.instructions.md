---
applyTo: "**/*.ts,**/*.tsx"
---

# The Standard React + TypeScript + Vite — Error Handling

## Applies To
All TypeScript and TSX files — brokers, services, hooks, pages, and components.

## Rules — Do
- Brokers must expose external failures by throwing (tsr-errors-001)
- Foundation services must localize broker exceptions into domain exceptions (tsr-errors-002)
- View services may convert service errors into view-friendly error models (tsr-errors-003)
- Pages decide how to display errors (tsr-errors-004)
- Log unexpected errors through a logging broker (tsr-errors-007)

## Rules — Do Not
- Never let components inspect raw infrastructure errors such as `error.status === 404` (tsr-errors-005)
- Never swallow errors silently (tsr-errors-006)

## Defaults
- Components receive an error model or a view model error field — never a raw `Error` or HTTP response.
- Pages own the decision of whether to show an error summary, redirect, or retry.
