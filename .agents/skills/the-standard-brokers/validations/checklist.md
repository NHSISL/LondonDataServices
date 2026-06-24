# The Standard Brokers — Checklist

- [ ] ts-brokers-001: Broker contains zero business logic (no if/else for business rules, no validation).
- [ ] ts-brokers-002: Broker interface name is technology-agnostic.
- [ ] ts-brokers-003: Storage broker methods use Insert/Select/SelectAll/Update/Delete language.
- [ ] ts-brokers-004: API broker methods use Post/Get/Put/Delete language.
- [ ] ts-brokers-005: Queue broker methods use Enqueue/Dequeue language.
- [ ] ts-brokers-006: All broker parameters are primitive types or native models only.
- [ ] ts-brokers-007: Broker returns raw external response without transformation.
- [ ] ts-brokers-008: Configuration retrieved via injected IConfiguration, not static access.
- [ ] ts-brokers-009: Multi-entity broker is implemented as partial classes.
- [ ] ts-brokers-010: Breaking contract changes use SPAL versioning.
