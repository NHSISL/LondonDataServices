# The Standard React + TypeScript + Vite — Models — Checklist

- [ ] All foundation models contain only domain or API-facing fields (tsr-models-001)
- [ ] All view models are produced by view services and contain display-ready fields (tsr-models-002)
- [ ] All component prop models describe rendering input only (tsr-models-003)
- [ ] No raw API response model is passed directly as a component prop when shapes differ (tsr-models-004)
- [ ] No model contains methods, API calls, mutations, or navigation logic (tsr-models-005)
- [ ] Foundation models are placed in `src/models/foundations/`
- [ ] View models are placed in `src/models/views/`
- [ ] Prop models are placed in `src/models/components/`
