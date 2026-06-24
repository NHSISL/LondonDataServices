---
name: the-standard-csharp-methods
version: 0.1.0
csharp-standard-version: v0.8
applies-to: ["*.cs"]
depends-on: ["the-standard-csharp-files", "the-standard-csharp-classes"]
---

# C# Coding Standard — Methods

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All C# source files (*.cs) containing method declarations and call expressions.
0.1/ Who: Any engineer writing or reviewing C# methods in The Standard ecosystem.
0.2/ What: Governs method spacing, line length, parameter breaking, chaining, and return statement layout.
0.3/ Applies to: *.cs
0.4/ Version: C# Coding Standard v0.8
0.5/ Depends on: the-standard-csharp-files, the-standard-csharp-classes

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Insert one blank line before a `return` statement when other statements precede it — see rules/rules.md#tsc-csharp-methods-001
  2. Break method call chains that exceed 120 characters onto one argument per line — see rules/rules.md#tsc-csharp-methods-002
  3. Align broken parameters with the opening parenthesis — see rules/rules.md#tsc-csharp-methods-003
  4. Use named parameters when calling a method with literal arguments — see rules/rules.md#tsc-csharp-methods-004
  5. Keep method bodies to a single level of abstraction — see rules/rules.md#tsc-csharp-methods-005
  6. Stack multiple single-argument calls on consecutive lines without blank lines between them — see rules/rules.md#tsc-csharp-methods-006

1.1/ Don'ts:
  1. Never exceed 120 characters per line — see validations/anti-patterns.md#line-too-long
  2. Never omit the blank line before `return` when the method has more than one statement — see validations/anti-patterns.md#missing-return-blank-line
  3. Never use abbreviated method names (e.g., `CalcTtl` instead of `CalculateTotal`) — see validations/anti-patterns.md#abbreviated-name
  4. Never chain calls on a single line when the result exceeds 120 characters — see validations/anti-patterns.md#long-chain
  5. Never mix uglification (line breaks) inside an already-beautified (chained) expression — see validations/anti-patterns.md#mixed-chaining

1.2/ Ask:
  - Ask when it is unclear whether breaking a call chain improves or reduces readability for chains under 120 characters.

1.3/ Defaults:
  - When a method has exactly one statement and that statement is `return`, no blank line is inserted before it.
  - When all parameters fit on one line within 120 characters, do not break them.

1.4/ Examples:
  - ✅ see examples/good/example_good_methods.cs
  - ❌ see examples/bad/example_bad_methods.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Corrected C# source with compliant method declarations and call expressions.
2.1/ Outcome: Methods are readable, under 120 characters per line, with correct blank lines and parameter alignment.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsc-csharp-methods-001). No prose justification unless asked.
