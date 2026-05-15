# The Standard Testing — Anti-Patterns

## Implementation Before Test

**Violates:** ts-testing-001
**What happens:** The service method is written in full before any test is created; tests are added afterward to verify the existing code.
**Why it's wrong:** Tests written after implementation verify what the code does, not what it should do. Edge cases, validations, and exception paths are routinely skipped.
**Fix:** Delete the implementation. Write a failing test first. Implement only enough to make it pass. Repeat.

## Hardcoded Values

**Violates:** ts-testing-005
**What happens:** A test creates `new Student { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Hassan" }`.
**Why it's wrong:** An implementation can pass such a test by returning the hard-coded values directly, masking missing logic. Randomization forces a general solution.
**Fix:** Use `new Filler<Student>().Create()` or a `CreateRandomStudent()` helper method.

## Missing VerifyNoOtherCalls

**Violates:** ts-testing-007
**What happens:** The test verifies that `InsertStudentAsync` was called once but does not call `VerifyNoOtherCalls()`.
**Why it's wrong:** Without `VerifyNoOtherCalls()`, extra unexpected broker or service calls (e.g., a logging call that shouldn't happen) pass undetected.
**Fix:** Add `this.storageBrokerMock.VerifyNoOtherCalls();` and `this.loggingBrokerMock.VerifyNoOtherCalls();` at the end of every test.

## Shared Test State

**Violates:** ts-testing-005
**What happens:** A static `Student` field is initialized once and reused across multiple test methods.
**Why it's wrong:** Shared mutable state causes tests to become order-dependent. A mutation in one test changes behavior in another.
**Fix:** Create a new random instance inside each test method using `CreateRandomStudent()`.
