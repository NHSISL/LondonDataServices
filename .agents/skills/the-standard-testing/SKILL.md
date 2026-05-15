---
name: the-standard-testing
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*Tests*.cs"]
depends-on: ["the-standard-core", "the-standard-foundations", "the-standard-tst-commits", "the-standard-tst-pull-requests", "the-standard-tst-projects"]
---

# The Standard — Testing

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All test projects in Standard-compliant systems.
0.1/ Who: Engineers writing or reviewing unit tests for any layer.
0.2/ What: Enforces TDD discipline, test naming, FAIL/PASS cycle, randomization, mocking, and test organization.
0.3/ Applies to: *Tests*.cs
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-foundations

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Write a failing test (FAIL) before writing any implementation code (TDD) → see rules/rules.md#ts-testing-001
  2. Confirm the test fails for the right reason before writing implementation → see rules/rules.md#ts-testing-002
  3. Name tests using the pattern `ShouldDo{Outcome}[When{Condition}]Async` → see rules/rules.md#ts-testing-003
  4. Structure every test with Given/When/Then comments → see rules/rules.md#ts-testing-004
  5. Randomize all inputs and outputs; use `Filler<T>` or equivalent → see rules/rules.md#ts-testing-005
  6. Use `DeepClone()` on the expected variable to dereference it from inputs → see rules/rules.md#ts-testing-006
  7. Verify mock calls with `Times.Once` and `VerifyNoOtherCalls()` at the end of every test → see rules/rules.md#ts-testing-007
  8. Organize tests in partial classes mirroring the service partial classes → see rules/rules.md#ts-testing-008

1.1/ Don'ts:
  1. Must not write implementation before a failing test exists → see validations/anti-patterns.md#impl-before-test
  2. Must not use hard-coded values where randomization is possible → see validations/anti-patterns.md#hardcoded-values
  3. Must not skip `VerifyNoOtherCalls()` — all unexpected interactions must be caught → see validations/anti-patterns.md#missing-verify-no-other-calls
  4. Must not share mutable state between tests → see validations/anti-patterns.md#shared-test-state

1.2/ Ask:
  - Ask when a test requires a specific value (not random) — confirm the business reason.

1.3/ Defaults:
  - Default test framework: xUnit.
  - Default mocking library: Moq.
  - Default assertion library: FluentAssertions.
  - Default randomization: `Tynamix.ObjectFiller` (`Filler<T>`).

1.4/ Examples:
  - ✅ see examples/good/example_good_unit_test.cs
  - ❌ see examples/bad/example_bad_hardcoded_test.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# xUnit test files.
2.1/ Outcome: Tests that are randomized, well-named, correctly structured with Given/When/Then, and fully verify mock interactions.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
