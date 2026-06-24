---
applyTo: "**/*.cs"
---

# C# Coding Standard — Variables

## Applies To
All C# source files containing local variable, field, and parameter declarations.

## Rules — Do
- Use full English words for all variable names (tsc-csharp-variables-001)
- Use plural names for collections (tsc-csharp-variables-002)
- Use `var` when the right-hand type is immediately obvious from the declaration (tsc-csharp-variables-004)
- Use the explicit type when the right-hand type is not immediately obvious (tsc-csharp-variables-005)
- Break a declaration onto multiple lines when it exceeds 120 characters (tsc-csharp-variables-006)
- Leave one blank line between logically distinct variable declaration blocks (tsc-csharp-variables-007)

## Rules — Do Not
- Never use abbreviations in variable names — `std`, `stud`, `cnt` are forbidden (tsc-csharp-variables-001)
- Never embed the type in the variable name — `studentList`, `studentArray` are forbidden (tsc-csharp-variables-002)
- Never name null/default placeholder variables with a generic term (tsc-csharp-variables-003)
- Never use `var` when the type is not immediately clear from the right-hand side (tsc-csharp-variables-004)
- Never declare multiple variables on the same line (tsc-csharp-variables-005)

## Defaults
- `new` expressions make the type obvious — use `var`.
- Method return values whose return type is not in the method name — use explicit type.
- Null/default placeholder variables must be named `maybe{Entity}` — e.g., `maybeStudent`.
