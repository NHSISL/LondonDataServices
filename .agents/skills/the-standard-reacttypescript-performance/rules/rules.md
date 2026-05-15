# The Standard React + TypeScript + Vite — Performance — Rules

## Memoisation Policy

**tsr-performance-001** [WARNING] Do not memoize by default. `useMemo`, `useCallback`, and `React.memo` must only be applied when a measured performance problem exists (e.g., profiler trace, production metric). Speculative memoisation adds cognitive overhead, hides re-render bugs, and rarely improves real-world performance.

## Object and Function Creation in Hot Render Paths

**tsr-performance-002** [WARNING] Do not create large objects, arrays, or functions inline inside the render function of components that re-render frequently. Define stable constants at module scope or with `useMemo`/`useCallback` only when profiler evidence justifies it.

## Component Size

**tsr-performance-003** [WARNING] Avoid oversized components. A component that renders a very large subtree is difficult to optimise later. Split large render trees into smaller, focused components at natural responsibility boundaries, which also enables more targeted memoisation when needed.

## Context Updates

**tsr-performance-004** [WARNING] Avoid unnecessary context updates. A context that holds many unrelated values causes all consumers to re-render whenever any value changes. Split contexts by update frequency (e.g., auth context, theme context, and domain data context are separate providers).

## Bundle Size

**tsr-performance-005** [ERROR] Bundle size increases must be reviewed before a dependency is added. Use `vite-bundle-visualizer` or an equivalent tool to understand the size impact. Prefer tree-shakeable libraries and avoid importing an entire library when only a small subset is used.

## Lazy Loading Routes

**tsr-performance-006** [WARNING] Lazy-load route-level features when the bundle impact is meaningful. Use `React.lazy` + `Suspense` to split route pages into separate chunks so users do not download code for routes they may not visit.
