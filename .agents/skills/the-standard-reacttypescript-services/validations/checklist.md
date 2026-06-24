# The Standard React + TypeScript + Vite — Foundation Services — Checklist

- [ ] Service imports only brokers and models — no React, no view services, no pages (tsr-foundation-001, tsr-foundation-002)
- [ ] No JSX is present anywhere in the service file (tsr-foundation-003)
- [ ] All inputs are validated before any broker call (tsr-foundation-004)
- [ ] Broker exceptions are caught and re-thrown as domain exceptions (tsr-foundation-005)
- [ ] No navigation calls exist in the service (tsr-foundation-006)
- [ ] No `useState`, `useReducer`, or context mutations are present (tsr-foundation-007)
- [ ] Service is split into `{domain}Service.ts`, `{domain}Service.validations.ts`, `{domain}Service.exceptions.ts`
- [ ] Service has a corresponding `i{domain}Service.ts` interface file
