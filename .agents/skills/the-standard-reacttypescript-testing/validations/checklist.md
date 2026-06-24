# The Standard React + TypeScript + Vite — Testing — Checklist

- [ ] Broker contract is covered by interface-based or integration tests (tsr-testing-001)
- [ ] Foundation service has tests for happy path, validation failure, and exception localization (tsr-testing-002)
- [ ] View service has tests for aggregation and view model field mapping (tsr-testing-003)
- [ ] Component tests query by role, label, or visible text — not by class names or data-testid overuse (tsr-testing-004)
- [ ] Page tests cover loading, error, empty, and success states (tsr-testing-005)
- [ ] No test asserts on state variable names, hook call counts, or React refs (tsr-testing-006)
- [ ] No large snapshot used as sole correctness proof (tsr-testing-007)
- [ ] Mocks placed only at broker or service interface boundaries (tsr-testing-008)
