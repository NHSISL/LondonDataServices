---
name: the-standard-csharp-directives
version: 0.1.0
csharp-standard-version: v0.8
applies-to: ["*.cs"]
depends-on: ["the-standard-csharp-files"]
---

# C# Coding Standard — Directives

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All C# source files (*.cs).
0.1/ Who: Any engineer writing or reviewing C# code in The Standard ecosystem.
0.2/ What: Governs the ordering, grouping, and hygiene of `using` directives at the top of every C# file.
0.3/ Applies to: *.cs
0.4/ Version: C# Coding Standard v0.8
0.5/ Depends on: the-standard-csharp-files

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Place all `using` directives at the top of the file, outside any namespace — see rules/rules.md#tsc-csharp-directives-001
  2. Order groups: System namespaces first, third-party next, internal/project last — see rules/rules.md#tsc-csharp-directives-002
  3. Separate each group with exactly one blank line — see rules/rules.md#tsc-csharp-directives-003
  4. Sort directives alphabetically within each group — see rules/rules.md#tsc-csharp-directives-004
  5. Use one `using` directive per line — see rules/rules.md#tsc-csharp-directives-005

1.1/ Don'ts:
  1. Never leave unused `using` directives — see validations/anti-patterns.md#unused-using
  2. Never mix groups without a blank-line separator — see validations/anti-patterns.md#mixed-groups
  3. Never place `using` directives inside a namespace block — see validations/anti-patterns.md#using-inside-namespace
  4. Never alias a namespace unless required to resolve an ambiguity — see validations/anti-patterns.md#unnecessary-alias
  5. Never use `using static` except where the Standard explicitly permits it — see validations/anti-patterns.md#using-static

1.2/ Ask:
  - Ask when it is unclear whether a namespace is "third-party" or "internal/project" in a monorepo.

1.3/ Defaults:
  - When only one group is present, no blank-line separator is needed.
  - `Microsoft.*` namespaces are treated as third-party (not System), unless they are `Microsoft.Extensions.*` already included in the SDK.

1.4/ Examples:
  - ✅ see examples/good/example_good_directives.cs
  - ❌ see examples/bad/example_bad_directives.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Corrected C# source file with properly ordered and grouped `using` directives.
2.1/ Outcome: All directives are ordered, grouped, and hygienically clean — no unused, no mixed groups, no directives inside namespaces.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsc-csharp-directives-002). No prose justification unless asked.
