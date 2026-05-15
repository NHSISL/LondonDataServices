---
applyTo: "src/services/foundations/**/*"
---

# The Standard React + TypeScript + Vite — Foundation Services

## Applies To
All foundation service classes, validation partials, exception partials, and interfaces in `src/services/foundations/**/*`.

## Rules — Do
- Foundation services must depend only on brokers, models, and approved same-layer dependencies (tsr-foundation-001)
- Foundation services must validate inputs before calling brokers (tsr-foundation-004)
- Foundation services must localize external exceptions into domain-specific exceptions (tsr-foundation-005)

## Rules — Do Not
- Never import React, `useState`, `useEffect`, or any React hook in a foundation service (tsr-foundation-002)
- Never return JSX from a foundation service (tsr-foundation-003)
- Never perform page navigation from a foundation service (tsr-foundation-006)
- Never own React state in a foundation service (tsr-foundation-007)

## Defaults
- Foundation services split across three files: `{domain}Service.ts`, `{domain}Service.validations.ts`, `{domain}Service.exceptions.ts`.
- Validation failures throw before any broker call is made.
