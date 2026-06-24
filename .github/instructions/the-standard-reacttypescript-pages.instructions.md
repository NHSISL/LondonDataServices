---
applyTo: "src/pages/**/*"
---

# The Standard React + TypeScript + Vite — Pages

## Applies To
All route-level page components and their page hooks in `src/pages/**/*`.

## Rules — Do
- A page must represent a route or screen (tsr-pages-001)
- A page may call a view service through its page hook (tsr-pages-002)
- A page must coordinate loading, error, and empty states (tsr-pages-005)
- A page must compose components to render its content (tsr-pages-006)
- A page must delegate complex data preparation to a view service (tsr-pages-007)

## Rules — Do Not
- Never call brokers directly from a page (tsr-pages-003)
- Never put business rules inside a page (tsr-pages-004)

## Defaults
- Each page has a corresponding `use{Domain}Page.ts` hook that handles state and view service calls.
- The page body is responsible only for rendering based on state returned by the hook.
