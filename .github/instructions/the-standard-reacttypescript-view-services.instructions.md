---
applyTo: "src/services/views/**/*"
---

# The Standard React + TypeScript + Vite — View Services

## Applies To
All view service classes, hooks, validation partials, and interfaces in `src/services/views/**/*`.

## Rules — Do
- View services must depend only on foundation services and approved same-layer dependencies (tsr-viewservices-001)
- View services must transform foundation model results into view models for the UI (tsr-viewservices-002)
- View services must expose their behaviour as a React hook using `use{Domain}ViewService` (tsr-viewservices-003)
- View services must validate inputs before calling foundation services (tsr-viewservices-004)
- View services must localize foundation service exceptions into view-layer exceptions (tsr-viewservices-005)

## Rules — Do Not
- Never call brokers directly from a view service (tsr-viewservices-006)
- Never return JSX from a view service (tsr-viewservices-002)

## Defaults
- Three-file split: `{domain}ViewService.ts`, `{domain}ViewService.validations.ts`, `{domain}ViewService.exceptions.ts`.
- View model naming: `{Domain}View` e.g., `StudentView`.
