# C# Coding Standard — Variables — Anti-Patterns

## Abbreviation

**Violates:** tsc-csharp-variables-001
**What happens:** A variable is named `std`, `stud`, `cnt`, or `tmp`.
**Why it's wrong:** Abbreviations force the reader to decode intent. Full words are unambiguous.
**Fix:** Rename to `student`, `student`, `count`, or `temporaryStudent` as appropriate.

## Type in Name

**Violates:** tsc-csharp-variables-003
**What happens:** A variable is named `studentList`, `studentArray`, or `studentObj`.
**Why it's wrong:** The type suffix is redundant — the declared type already conveys this. When the type changes, the name becomes misleading.
**Fix:** Rename to the plural of the element type: `students`.

## Null Naming

**Violates:** tsc-csharp-variables-008
**What happens:** A nullable placeholder variable is named `nullStudent`, `defaultStudent`, or just `result`.
**Why it's wrong:** Generic or negation-based names do not convey that the variable holds an uncertain value. `maybe` makes the intent explicit.
**Fix:** Rename to `maybeStudent`.

## Var Ambiguous

**Violates:** tsc-csharp-variables-004 / tsc-csharp-variables-005
**What happens:** `var student = studentService.RetrieveStudentByIdAsync(id).Result;` uses `var` when the type is not obvious from the method name alone.
**Why it's wrong:** The reader must navigate to `RetrieveStudentByIdAsync` to confirm the return type.
**Fix:** Use the explicit type: `Student student = await studentService.RetrieveStudentByIdAsync(id);`.

## Multi Declaration

**Violates:** tsc-csharp-variables-009
**What happens:** `int count = 0, total = 0;` declares two variables on one line.
**Why it's wrong:** Multi-variable declarations reduce readability and make individual variables harder to comment or annotate.
**Fix:** Declare each variable on its own line.
