# C# Coding Standard — Variables — Rules

## Naming

**tsc-csharp-variables-001** [ERROR] Variable names must be full English words — no abbreviations. Use `student`, not `std` or `stud`.

**tsc-csharp-variables-002** [ERROR] Collection variables must be named with the plural form of the element type. Use `students`, not `studentList` or `studentArray`.

**tsc-csharp-variables-003** [ERROR] Variable names must not embed the type. Use `students` (not `studentList`), `student` (not `studentObj`).

**tsc-csharp-variables-008** [ERROR] A null or default placeholder variable must be named `maybe{Entity}` (e.g., `maybeStudent`).

## Type Declaration

**tsc-csharp-variables-004** [ERROR] Use `var` when the declared type is immediately obvious from the right-hand side (e.g., `new` expressions, casts, and literals).

**tsc-csharp-variables-005** [ERROR] Use the explicit type when the right-hand side does not make the type immediately obvious (e.g., method return values where the type is not visible in the name).

## Line Length and Layout

**tsc-csharp-variables-006** [ERROR] When a variable declaration exceeds 120 characters, the right-hand side must be broken to the next line and indented by one level.

**tsc-csharp-variables-007** [WARN]  Logically distinct groups of variable declarations must be separated by exactly one blank line.

**tsc-csharp-variables-009** [ERROR] Multiple variables must not be declared on the same line.
