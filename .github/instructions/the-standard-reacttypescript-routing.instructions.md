---
applyTo: "src/routes/**/*,**/routes.tsx"
---

# The Standard React + TypeScript + Vite — Routing

## Applies To
All route definitions and route guards in `src/routes/**/*` and `routes.tsx`.

## Rules — Do
- Routes must point to pages, not low-level components (tsr-routing-001)
- Route definitions must be centralized unless the architecture requires modular registration (tsr-routing-002)
- Route guards must delegate access decisions to services (tsr-routing-003)

## Rules — Do Not
- Never embed business authorization rules directly in JSX route definitions (tsr-routing-004)

## Defaults
- Route guards call a view service or authorization service to determine access.
- Centralized route file is `src/app/routes.tsx`.
