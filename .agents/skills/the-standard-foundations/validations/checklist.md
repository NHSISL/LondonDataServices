# The Standard Foundations — Checklist

- [ ] ts-foundations-001: All service methods use business language (Add/Retrieve/Modify/Remove), not storage language.
- [ ] ts-foundations-002: No business-workflow methods (Upsert, Process, etc.) exist on the foundation service.
- [ ] ts-foundations-003: Structural validation (null, empty Guid, whitespace) is present for all inputs.
- [ ] ts-foundations-004: Logical validation is present for all applicable business rules.
- [ ] ts-foundations-005: The service integrates with exactly one entity broker.
- [ ] ts-foundations-006: All broker exceptions are caught and wrapped in dependency/service exception models.
- [ ] ts-foundations-007: Every behavior has a failing test written before the implementation.
- [ ] ts-foundations-008: All test inputs/outputs are randomized; expected variables use DeepClone().
