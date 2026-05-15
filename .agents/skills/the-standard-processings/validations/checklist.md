# The Standard Processing Services — Checklist

- [ ] ts-processings-001: Service combines at least two foundation primitives into a workflow.
- [ ] ts-processings-002: Exposed methods use higher-order business verbs, not raw CRUD verbs.
- [ ] ts-processings-003: No broker is injected or called directly.
- [ ] ts-processings-004: Existence checks or conditional writes are handled inside the processing service.
- [ ] ts-processings-005: Every behavior has a failing test before implementation.
- [ ] ts-processings-006: All test inputs/outputs are randomized; expected variables use DeepClone().
