# C# Coding Standard — Comments and Documentation — Anti-Patterns

## Block Comment Copyright

**Violates:** tsc-csharp-comments-005
**What happens:** The copyright header is written using `/* */` block comment syntax.
**Why it's wrong:** The C# Coding Standard requires `//` line comments for copyright blocks. Block comment syntax is not permitted.
**Fix:** Replace the block comment with the required dashed-line `//` format.

## XML Copyright Tag

**Violates:** tsc-csharp-comments-006
**What happens:** The copyright uses `//----------------------------------------------------------------` with `<copyright file="..." company="...">` XML tags inside a comment block.
**Why it's wrong:** XML copyright tags are the old Visual Studio style and are explicitly forbidden by the Standard.
**Fix:** Use the exact dashed-line `//` format without XML tags.

## Redundant Comment

**Violates:** tsc-csharp-comments-004
**What happens:** A comment reads `// Add the student to the database` immediately above `await this.storageBroker.InsertStudentAsync(student);`.
**Why it's wrong:** The code already expresses this intent unambiguously. The comment adds noise without adding information.
**Fix:** Remove the comment. Reserve comments for logic that code cannot express on its own.
