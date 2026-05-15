# The Standard Testing — Checklist

- [ ] ts-testing-001: Every behavior has a failing test written before the implementation.
- [ ] ts-testing-002: Failing test was confirmed to fail for the right reason before implementing.
- [ ] ts-testing-003: All test names follow `ShouldDo{Outcome}[When{Condition}]Async` pattern.
- [ ] ts-testing-004: Every test has `// given`, `// when`, `// then` comment sections.
- [ ] ts-testing-005: All input values are randomized (no hard-coded domain values).
- [ ] ts-testing-006: Expected variable is created with `DeepClone()`.
- [ ] ts-testing-007: All mock calls verified with `Times.Once`; `VerifyNoOtherCalls()` called on all mocks.
- [ ] ts-testing-008: Test class is partial and mirrors the structure of the system under test.
