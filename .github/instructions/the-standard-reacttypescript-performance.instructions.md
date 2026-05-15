---
applyTo: "**/*.tsx,vite.config.ts"
---

# The Standard React + TypeScript + Vite — Performance

## Applies To
All React TSX component files and Vite configuration — `*.tsx`, `vite.config.ts`.

## Rules — Do
- Profile before optimising — apply memoisation only when a measured problem exists (tsr-performance-001)
- Define stable objects and functions outside of the render function when they are expensive (tsr-performance-002)
- Split oversized components into smaller focused components (tsr-performance-003)
- Keep Context providers narrow — split contexts that update at different frequencies (tsr-performance-004)
- Review bundle size impact before adding dependencies (tsr-performance-005)
- Lazy-load route-level features when bundle impact is meaningful (tsr-performance-006)

## Rules — Do Not
- Never memoize by default without a measured justification (tsr-performance-001)
- Never create large objects or functions on every render inside a hot render path (tsr-performance-002)
- Never add heavy dependencies without reviewing bundle delta (tsr-performance-003)

## Defaults
- No `useMemo`, `useCallback`, or `React.memo` unless a measured problem is present.
- Stable constants defined at module scope or in a separate constants file.
- Context split by update frequency: auth context vs. theme context vs. data context.
