# The Standard React + TypeScript + Vite — Review — Anti-Patterns

## Opinion Comment Without Rule Citation

**Violates:** tsr-review-001, tsr-review-003
**What happens:** Reviewer writes "I'd prefer this to be a named function rather than an arrow function — it reads cleaner."
**Why it's wrong:** There is no Standard rule requiring named functions over arrow functions in this context. This is personal preference masquerading as a review concern.
**Fix:** Omit the comment, or cite the rule if one exists: `INFO tsr-typescript-XXX / ...`

## Architectural Error Logged as INFO

**Violates:** tsr-review-002
**What happens:** A component calls a broker directly, but the reviewer logs it as `INFO` with a note "might be better to use a service here".
**Why it's wrong:** A layer bypass is an architectural violation — it must be `ERROR` and must block merge.
**Fix:** `ERROR tsr-broker-001 / StudentComponent calls StudentBroker.getStudents directly / Components must only call view services, never brokers / Introduce a StudentViewService that wraps the broker call`

## Approving Non-Compliant Generated Code

**Violates:** tsr-review-008
**What happens:** A Copilot-generated component is approved without review because "it was auto-generated and looks fine".
**Why it's wrong:** Generated code is subject to the same Standard rules as hand-written code. Violations must be flagged regardless of source.
**Fix:** Review every generated file against all applicable Standard rules. Flag violations with the standard comment format.

## Missing Layer Identification

**Violates:** tsr-review-004
**What happens:** `ERROR tsr-async-002 / await used incorrectly / use .catch() / remove the await`
**Why it's wrong:** The comment does not identify which layer is affected, making it harder for the author to locate and understand the violation in context.
**Fix:** `ERROR tsr-async-002 / [Foundation Service layer] StudentFoundationService.addStudentAsync does not handle the rejected promise / Foundation services must handle and wrap exceptions / Add a try/catch and throw a StudentServiceException`
