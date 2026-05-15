# The Standard Orchestration Services — Checklist

- [ ] ts-orchestrations-001: Service coordinates at least two foundation/processing services for a multi-entity workflow.
- [ ] ts-orchestrations-002: Exposed methods use business-operation names reflecting the multi-entity workflow.
- [ ] ts-orchestrations-003: No broker is injected or called directly.
- [ ] ts-orchestrations-004: Cross-entity validation and business rules are handled in the orchestration service.
- [ ] ts-orchestrations-005: Every behavior has a failing test before implementation.
- [ ] ts-orchestrations-006: All test inputs/outputs are randomized; expected variables use DeepClone().
