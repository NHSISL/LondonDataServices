# The Standard React + TypeScript + Vite — Performance — Checklist

- [ ] No speculative `useMemo` / `useCallback` / `React.memo` — all present memoisation has a profiler-backed justification (tsr-performance-001)
- [ ] No large objects or functions created inline inside hot render paths without measured justification (tsr-performance-002)
- [ ] No oversized components — large render trees have been split at responsibility boundaries (tsr-performance-003)
- [ ] Context providers are split by update frequency — unrelated values are not grouped in a single context (tsr-performance-004)
- [ ] Bundle size delta reviewed before any new dependency was added (tsr-performance-005)
- [ ] Route-level features are lazy-loaded with `React.lazy` + `Suspense` where bundle impact is meaningful (tsr-performance-006)
