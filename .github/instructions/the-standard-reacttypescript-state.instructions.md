---
applyTo: "**/*.ts,**/*.tsx"
---

# The Standard React + TypeScript + Vite — State Management

## Applies To
All TypeScript and TSX files that declare or consume React state.

## Rules — Do
- Keep state as local as possible (tsr-state-001)
- Use URL state for route-significant state: shareable tabs, bookmarkable filters, route-relevant pagination (tsr-state-003)
- Use component state for local UI state: modal open/closed, pre-submit input, dropdown (tsr-state-004)
- Use application state only for cross-cutting concerns: authenticated user, global theme, feature flags (tsr-state-005)
- State transitions with business meaning must go through services (tsr-state-007)

## Rules — Do Not
- Never duplicate server data across multiple independent component states (tsr-state-002)
- Never use global state to avoid designing props (tsr-state-006)

## Defaults
- Component state is the default. Elevate only when cross-component sharing is genuinely required.
- Server data belongs in the page hook, not duplicated into child component state.
