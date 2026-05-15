# The Standard React + TypeScript + Vite — Brokers — Checklist

- [ ] Each broker wraps exactly one external concern (tsr-brokers-001)
- [ ] No broker contains conditional logic based on domain meaning (tsr-brokers-002)
- [ ] No broker returns component-shaped or view-model-shaped data (tsr-brokers-003)
- [ ] Mechanical serialization is limited to what the external dependency requires (tsr-brokers-004)
- [ ] Every broker has a corresponding interface file (tsr-brokers-005)
- [ ] No broker imports or calls a service, page, or component (tsr-brokers-006)
- [ ] Broker files are placed in `src/brokers/{kind}/`
