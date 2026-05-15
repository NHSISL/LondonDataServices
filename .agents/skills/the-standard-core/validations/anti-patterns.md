# The Standard Core — Anti-Patterns

## God Objects

**Violates:** ts-core-001
**What happens:** A single service class implements Add, Retrieve, Modify, Remove AND business-specific workflows AND sends emails.
**Why it's wrong:** It violates single responsibility and makes the class impossible to test, replace, or reason about.
**Fix:** Split into a foundation service (CRUD), a processing service (business workflows), and a notification broker.

## Poor Naming

**Violates:** ts-core-004
**What happens:** Variables and methods are named `stud`, `svc`, `BL`, `Mgr`, or `ProcessData()`.
**Why it's wrong:** Names that require mental decoding increase cognitive load and hide intent.
**Fix:** Use `student`, `studentService`, `businessLogic` (or better, domain-specific verbs like `AddStudentAsync`).

## Skipping Simulation (No TDD)

**Violates:** ts-core-003
**What happens:** Implementation is written first; tests are added afterward as an afterthought.
**Why it's wrong:** Tests written after implementation verify what the code does, not what it should do; edge cases and validations are routinely missed.
**Fix:** Write a failing test first, confirm it fails for the right reason, then implement only enough code to make it pass.

## Layer Bypass

**Violates:** ts-core-008
**What happens:** A controller calls `storageBroker.SelectStudentByIdAsync()` directly.
**Why it's wrong:** It skips validation, logging, and all service-layer responsibilities, and couples the exposer to the infrastructure.
**Fix:** Controllers must call services only; services call brokers; brokers call external resources.

## Technical Wrapper Domain Models

**Violates:** ts-core-007
**What happens:** A domain model `ApiResult<Student>` wraps the real model instead of `Student` being passed directly.
**Why it's wrong:** `ApiResult<T>` has no real-world meaning; it is a technical artifact that leaks infrastructure concerns into the domain.
**Fix:** Pass `Student` directly across layer boundaries; handle HTTP semantics in the exposer layer only.
