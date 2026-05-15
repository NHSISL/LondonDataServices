---
applyTo: "**/*.cs"
---

# C# Coding Standard — Comments and Documentation

## Applies To
All C# source files requiring comments, copyright headers, or method documentation.

## Rules — Do
- Use comments only to explain non-obvious intent, inaccessible code, or complex logic that code cannot express (tsc-csharp-comments-001)
- Format copyright blocks using the exact dashed-line pattern (tsc-csharp-comments-002)
- Document methods performing inaccessible or complex operations with: Purposing, Incomes, Outcomes, and Side Effects (tsc-csharp-comments-003)

## Rules — Do Not
- Must not use `/* */` block comment syntax for copyright headers (tsc-csharp-comments-001)
- Must not use XML `<copyright>` tags for copyright headers (tsc-csharp-comments-002)
- Must not add comments that merely restate what the code already clearly expresses (tsc-csharp-comments-003)

## Defaults
- Every file containing production code must start with the copyright header block.
- Test files follow the same copyright rule.
- If the intent is clear from the method name and types, a comment is not needed.
