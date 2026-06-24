---
applyTo: "**/*Tests*.cs"
---

# The Standard — Testing

## Applies To
All test projects in Standard-compliant systems — `*Tests*.cs`.

## Rules — Do
- Write a failing test before writing any implementation code (ts-testing-001)
- Confirm the test fails for the right reason before writing implementation (ts-testing-002)
- Name tests using the pattern `ShouldDo{Outcome}[When{Condition}]Async` (ts-testing-003)
- Structure every test with Given/When/Then comments (ts-testing-004)
- Randomize all inputs and outputs using `Filler<T>` or equivalent (ts-testing-005)
- Use `DeepClone()` on the expected variable to dereference it from inputs (ts-testing-006)
- Verify mock calls with `Times.Once` and `VerifyNoOtherCalls()` at the end of every test (ts-testing-007)
- Organize tests in partial classes mirroring the service partial classes (ts-testing-008)

## Rules — Do Not
- Must not write implementation before a failing test exists (ts-testing-001)
- Must not use hard-coded values where randomization is possible (ts-testing-002)
- Must not skip `VerifyNoOtherCalls()` — all unexpected interactions must be caught (ts-testing-003)
- Must not share mutable state between tests (ts-testing-004)

## Defaults
- Default test framework: xUnit.
- Default mocking library: Moq.
- Default assertion library: FluentAssertions.
- Default randomization: `Tynamix.ObjectFiller` (`Filler<T>`).
