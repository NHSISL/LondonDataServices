# The Standard React + TypeScript + Vite — TypeScript — Checklist

- [ ] `tsconfig.json` has `strict: true`, `noImplicitAny`, `strictNullChecks`, `noUncheckedIndexedAccess`, `exactOptionalPropertyTypes` (tsr-typescript-001)
- [ ] All service, broker, and hook signatures have explicit parameter and return types (tsr-typescript-002)
- [ ] Data shapes use `type`, behavioral contracts use `interface` (tsr-typescript-003)
- [ ] No `any` is present in any file (tsr-typescript-004)
- [ ] All services, brokers, models, and hooks use named exports (tsr-typescript-005)
- [ ] No barrel `index.ts` that hides which architectural layer is consumed (tsr-typescript-006)
