---
name: the-standard-csharp-files
version: 0.1.0
csharp-standard-version: v0.8
applies-to: ["*.cs"]
depends-on: ["none"]
---

# C# Coding Standard — Files

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All C# source files in any Standard-compliant project.
0.1/ Who: Engineers creating or renaming C# source files.
0.2/ What: Enforces file naming conventions and partial class file naming rules.
0.3/ Applies to: *.cs
0.4/ Version: v0.8
0.5/ Depends on: none

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Name every `.cs` file using PascalCase followed by the `.cs` extension → see rules/rules.md#tsc-csharp-files-001
  2. Name partial class files as `{RootClass}.{Aspect}.cs` (e.g., `StudentService.Validations.cs`) → see rules/rules.md#tsc-csharp-files-002
  3. Name partial class files with an additional sub-aspect as `{RootClass}.{Aspect}.{SubAspect}.cs` (e.g., `StudentService.Validations.Add.cs`) → see rules/rules.md#tsc-csharp-files-003

1.1/ Don'ts:
  1. Must not use camelCase or all-lowercase file names (e.g., `student.cs`, `studentService.cs`) → see validations/anti-patterns.md#lowercase-filename
  2. Must not use underscores or hyphens to separate words in file names (e.g., `Student_Service.cs`) → see validations/anti-patterns.md#underscore-filename
  3. Must not concatenate aspects without a dot separator (e.g., `StudentServiceValidations.cs`) → see validations/anti-patterns.md#no-dot-separator
  4. Must not use an underscore separator for partial class files (e.g., `StudentService_Validations.cs`) → see validations/anti-patterns.md#underscore-partial

1.2/ Ask:
  - Ask when a file contains more than one top-level class — confirm whether it should be split.

1.3/ Defaults:
  - One class per file. File name matches the class name exactly.
  - Partial class files use dot notation: `{Root}.{Aspect}.cs`.

1.4/ Examples:
  - ✅ `Student.cs`, `StudentService.cs`, `StudentService.Validations.cs`, `StudentService.Validations.Add.cs`
  - ❌ `student.cs`, `studentService.cs`, `Student_Service.cs`, `StudentServiceValidations.cs`, `StudentService_Validations.cs`

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Correctly named `.cs` file.
2.1/ Outcome: All C# files follow PascalCase naming; partial class files use dot-separated aspect notation.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
