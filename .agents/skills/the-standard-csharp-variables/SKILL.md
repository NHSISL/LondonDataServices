---
name: the-standard-csharp-variables
version: 0.1.0
csharp-standard-version: v0.8
applies-to: ["*.cs"]
depends-on: ["the-standard-csharp-files", "the-standard-csharp-classes"]
---

# C# Coding Standard — Variables

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All C# source files (*.cs) containing local variable, field, and parameter declarations.
0.1/ Who: Any engineer writing or reviewing C# variable declarations in The Standard ecosystem.
0.2/ What: Governs naming conventions, type declaration style (`var` vs. explicit), plural forms for collections, line-length breakdown, and multi-declaration spacing.
0.3/ Applies to: *.cs
0.4/ Version: C# Coding Standard v0.8
0.5/ Depends on: the-standard-csharp-files, the-standard-csharp-classes

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Use full English words for all variable names — see rules/rules.md#tsc-csharp-variables-001
  2. Use plural names for collections — see rules/rules.md#tsc-csharp-variables-002
  3. Use `var` when the right-hand type is immediately obvious from the declaration — see rules/rules.md#tsc-csharp-variables-004
  4. Use the explicit type when the right-hand type is not immediately obvious — see rules/rules.md#tsc-csharp-variables-005
  5. Break a declaration onto multiple lines when it exceeds 120 characters — see rules/rules.md#tsc-csharp-variables-006
  6. Leave one blank line between logically distinct variable declaration blocks — see rules/rules.md#tsc-csharp-variables-007

1.1/ Don'ts:
  1. Never use abbreviations in variable names (e.g., `std`, `stud`, `cnt`) — see validations/anti-patterns.md#abbreviation
  2. Never embed the type in the variable name (e.g., `studentList`, `studentArray`) — see validations/anti-patterns.md#type-in-name
  3. Never name null/default placeholder variables with a generic term — see validations/anti-patterns.md#null-naming
  4. Never use `var` when the type is not immediately clear from the right-hand side — see validations/anti-patterns.md#var-ambiguous
  5. Never declare multiple variables on the same line — see validations/anti-patterns.md#multi-declaration

1.2/ Ask:
  - Ask when it is unclear whether the right-hand side of a declaration makes the type "immediately obvious".

1.3/ Defaults:
  - `new` expressions make the type obvious → use `var`.
  - Method return values whose return type is not in the method name → use explicit type.
  - Null/default placeholder variables must be named `maybe{Entity}` (e.g., `maybeStudent`).

1.4/ Examples:
  - ✅ see examples/good/example_good_variables.cs
  - ❌ see examples/bad/example_bad_variables.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Corrected C# source with compliant variable declarations.
2.1/ Outcome: All variables are clearly named, correctly typed, and properly spaced.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsc-csharp-variables-001). No prose justification unless asked.
