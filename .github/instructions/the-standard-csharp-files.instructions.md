---
applyTo: "**/*.cs"
---

# C# Coding Standard — Files

## Applies To
All C# source files — naming and partial class file conventions.

## Rules — Do
- Name every `.cs` file using PascalCase followed by the `.cs` extension (tsc-csharp-files-001)
- Name partial class files as `{RootClass}.{Aspect}.cs` — e.g., `StudentService.Validations.cs` (tsc-csharp-files-002)
- Name partial files with a sub-aspect as `{RootClass}.{Aspect}.{SubAspect}.cs` — e.g., `StudentService.Validations.Add.cs` (tsc-csharp-files-003)

## Rules — Do Not
- Must not use camelCase or all-lowercase file names — `student.cs`, `studentService.cs` are forbidden (tsc-csharp-files-001)
- Must not use underscores or hyphens to separate words — `Student_Service.cs` is forbidden (tsc-csharp-files-002)
- Must not concatenate aspects without a dot separator — `StudentServiceValidations.cs` is forbidden (tsc-csharp-files-003)
- Must not use an underscore separator for partial class files — `StudentService_Validations.cs` is forbidden (tsc-csharp-files-004)

## Defaults
- One class per file. File name matches the class name exactly.
- Partial class files use dot notation: `{Root}.{Aspect}.cs`.
