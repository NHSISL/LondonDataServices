---
applyTo: "**/*.cs"
---

# C# Coding Standard — Methods

## Applies To
All C# source files containing method declarations and call expressions.

## Rules — Do
- Insert one blank line before a `return` statement when other statements precede it (tsc-csharp-methods-001)
- Break method call chains exceeding 120 characters onto one argument per line (tsc-csharp-methods-002)
- Align broken parameters with the opening parenthesis (tsc-csharp-methods-003)
- Use named parameters when calling a method with literal arguments (tsc-csharp-methods-004)
- Keep method bodies to a single level of abstraction (tsc-csharp-methods-005)
- Stack multiple single-argument calls on consecutive lines without blank lines between them (tsc-csharp-methods-006)

## Rules — Do Not
- Never exceed 120 characters per line (tsc-csharp-methods-001)
- Never omit the blank line before `return` when the method has more than one statement (tsc-csharp-methods-002)
- Never use abbreviated method names — `CalcTtl` instead of `CalculateTotal` is forbidden (tsc-csharp-methods-003)
- Never chain calls on a single line when the result exceeds 120 characters (tsc-csharp-methods-004)
- Never mix line breaks inside an already-chained expression (tsc-csharp-methods-005)

## Defaults
- When a method has exactly one statement that is `return`, no blank line is inserted before it.
- When all parameters fit on one line within 120 characters, do not break them.
