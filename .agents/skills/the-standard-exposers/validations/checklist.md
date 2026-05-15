# The Standard Exposers — Checklist

- [ ] ts-exposers-001: Controller class name is plural (StudentsController, not StudentController).
- [ ] ts-exposers-002: HTTP verbs correctly map to service operation names (POST→Add, GET→Retrieve, PUT→Modify, DELETE→Remove).
- [ ] ts-exposers-003: All actions return the correct HTTP status codes.
- [ ] ts-exposers-004: All service exceptions are caught and mapped to IActionResult; none propagate uncaught.
- [ ] ts-exposers-005: No broker is injected or referenced in the controller.
- [ ] ts-exposers-006: Each action body contains exactly one service call plus exception mapping — no business logic.
