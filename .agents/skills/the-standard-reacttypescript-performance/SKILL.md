---
name: the-standard-reacttypescript-performance
version: 0.1.0
standard-version: v0.1.0
applies-to: ["*.tsx", "vite.config.ts"]
depends-on: ["the-standard-reacttypescript-components", "the-standard-reacttypescript-vite"]
---

# The Standard React + TypeScript + Vite — Performance

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All React TSX component files and Vite configuration.
0.1/ Who: Any engineer building, optimising, or reviewing UI components and build configuration in a Standard frontend project.
0.2/ What: Governs memoisation policy, object/function creation in render paths, component size, context update scope, bundle size, and lazy-loading strategy.
0.3/ Applies to: `*.tsx`, `vite.config.ts`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-components, the-standard-reacttypescript-vite

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Profile before optimising — apply memoisation only when a measured problem exists — see rules/rules.md#tsr-performance-001
  2. Define stable objects and functions outside of the render function when they are expensive — see rules/rules.md#tsr-performance-002
  3. Split oversized components into smaller focused components — see rules/rules.md#tsr-performance-003
  4. Keep Context providers narrow — split contexts that update at different frequencies — see rules/rules.md#tsr-performance-004
  5. Review bundle size impact before adding dependencies — see rules/rules.md#tsr-performance-005
  6. Lazy-load route-level features when bundle impact is meaningful — see rules/rules.md#tsr-performance-006

1.1/ Don'ts:
  1. Never memoize by default without a measured justification — see validations/anti-patterns.md#premature-memoisation
  2. Never create large objects or functions on every render inside a hot render path — see validations/anti-patterns.md#object-in-render
  3. Never add heavy dependencies without reviewing bundle delta — see validations/anti-patterns.md#unchecked-dependency

1.2/ Ask:
  - Ask when it is unclear whether a component is a hot render path requiring optimisation.

1.3/ Defaults:
  - Default: no `useMemo` / `useCallback` / `React.memo` unless a measured problem is present.
  - Default: stable constants defined at module scope or in a separate constants file.
  - Default: context split by update frequency (auth context vs. theme context vs. data context).

1.4/ Examples:
  - ✅ see examples/good/example_good_performance.tsx
  - ❌ see examples/bad/example_bad_performance.tsx

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: TSX components with no speculative memoisation, stable references, and focused context providers.
2.1/ Outcome: Bundle size is tracked, route-level lazy loading is applied where meaningful, and memoisation is applied only where profiling evidence justifies it.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-performance-001). No prose justification unless asked.
