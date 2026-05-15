# The Standard React + TypeScript + Vite — View Services — Checklist

- [ ] View service calls foundation services only — no direct broker calls (tsr-viewservices-001, tsr-viewservices-004)
- [ ] No JSX expressions or JSX return values in the service (tsr-viewservices-002)
- [ ] No React or React hook imports (tsr-viewservices-003)
- [ ] View model fields contain display text values, not CSS class names or component names (tsr-viewservices-006)
- [ ] View service has a corresponding interface file `i{domain}ViewService.ts`
- [ ] View service is placed in `src/services/views/{domain}/`
