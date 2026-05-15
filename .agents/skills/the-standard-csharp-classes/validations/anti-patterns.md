# C# Coding Standard — Classes and Interfaces — Anti-Patterns

## Model Suffix

**Violates:** tsc-csharp-classes-001
**What happens:** A domain model is named `StudentModel`, `StudentObj`, or `StudentEntity`.
**Why it's wrong:** The suffix adds no information — the class is already a model by nature. It pollutes the name and makes the domain language less clean.
**Fix:** Name it `Student`.

## BL Suffix

**Violates:** tsc-csharp-classes-002
**What happens:** A service class is named `StudentBL`, `StudentBusinessLogic`, or `StudentManager`.
**Why it's wrong:** These suffixes are legacy naming conventions not used in The Standard. The `Service` suffix is the only permitted business logic suffix.
**Fix:** Rename to `StudentService`.

## Underscore Field

**Violates:** tsc-csharp-classes-005
**What happens:** A class field is declared as `private readonly string _studentName;`.
**Why it's wrong:** The underscore prefix is a legacy convention. Fields must be camelCase without any prefix.
**Fix:** Rename to `private readonly string studentName;` and update all references.

## Missing This

**Violates:** tsc-csharp-classes-006
**What happens:** Inside a constructor or method, a private field `_studentName` is assigned without `this.`.
**Why it's wrong:** Without `this.`, it is ambiguous whether the assignment targets a field or a local variable. `this.` makes the intent explicit.
**Fix:** Always write `this.studentName = studentName;` when assigning to a private field.

## Positional Literals

**Violates:** tsc-csharp-classes-007
**What happens:** `new Student("Josh", 150)` passes literal values positionally with no named aliases.
**Why it's wrong:** Positional literals are unreadable — the reader must look up the constructor signature to understand what each value means.
**Fix:** Write `new Student(name: "Josh", score: 150)`.

## Target-Typed New

**Violates:** tsc-csharp-classes-009
**What happens:** `Student student = new (...);` uses target-typed new syntax.
**Why it's wrong:** The Standard requires `var` when the right-hand type is clear. Target-typed new reverses this by putting the type on the left. Use `var student = new Student(...)`.
**Fix:** Replace `Student student = new (...)` with `var student = new Student(...)`.
