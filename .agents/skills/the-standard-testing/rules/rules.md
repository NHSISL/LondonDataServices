# The Standard Testing — Rules

## TDD CYCLE

**ts-testing-001** [ERROR] A failing test (FAIL) must exist before any implementation code is written for a behavior.
**ts-testing-002** [ERROR] The failing test must be confirmed to fail for the correct reason (wrong assertion, not a compile error) before the implementation is written.

## NAMING

**ts-testing-003** [ERROR] Test method names must follow the pattern: `ShouldDo{Outcome}[When{Condition}]Async` (e.g., `ShouldAddStudentAsync`, `ShouldThrowValidationExceptionOnAddWhenStudentIsNullAsync`).

## STRUCTURE

**ts-testing-004** [ERROR] Every test must be structured with `// given`, `// when`, and `// then` comment sections.
**ts-testing-008** [ERROR] Test classes must be partial and organized to mirror the partial class structure of the system under test.

## RANDOMIZATION

**ts-testing-005** [ERROR] All test input values must be randomized using `Filler<T>` or an equivalent randomization library; hard-coded domain values are forbidden.
**ts-testing-006** [ERROR] Expected variables must be created using `DeepClone()` to dereference them from input variables.

## MOCK VERIFICATION

**ts-testing-007** [ERROR] Every test must end with `Times.Once` verification on expected mock calls and `VerifyNoOtherCalls()` on all mocks.
