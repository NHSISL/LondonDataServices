# The Standard React + TypeScript + Vite — Pages — Checklist

- [ ] Page represents a single route or screen (tsr-pages-001)
- [ ] Page calls its view service only through a `use{Domain}Page.ts` hook (tsr-pages-002)
- [ ] No `fetch()` or broker calls in the page component body (tsr-pages-003)
- [ ] No business-rule conditionals in the page (tsr-pages-004)
- [ ] Loading, error, and empty states are all handled explicitly (tsr-pages-005)
- [ ] Page content is rendered through named child components (tsr-pages-006)
- [ ] No inline data fetching, transformation, or aggregation in the page body (tsr-pages-007)
- [ ] Page and hook files are placed in `src/pages/{domain}/`
