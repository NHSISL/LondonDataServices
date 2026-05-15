# C# Coding Standard — Files — Anti-Patterns

## Lowercase Filename

**Violates:** tsc-csharp-files-001, tsc-csharp-files-004
**What happens:** A file is saved as `student.cs` or `studentService.cs`.
**Why it's wrong:** C# files must use PascalCase. Lowercase names break convention consistency and can cause case-sensitive filesystem issues.
**Fix:** Rename to `Student.cs` or `StudentService.cs`.

## Underscore Filename

**Violates:** tsc-csharp-files-004
**What happens:** A file is named `Student_Service.cs`.
**Why it's wrong:** Underscores are not a word separator in PascalCase file names.
**Fix:** Rename to `StudentService.cs`.

## No Dot Separator

**Violates:** tsc-csharp-files-005
**What happens:** A partial class file is named `StudentServiceValidations.cs`.
**Why it's wrong:** Without the dot separator, the relationship between the root class and the aspect is not visible from the file name.
**Fix:** Rename to `StudentService.Validations.cs`.

## Underscore Partial

**Violates:** tsc-csharp-files-002, tsc-csharp-files-004
**What happens:** A partial class file is named `StudentService_Validations.cs`.
**Why it's wrong:** The separator between the root class and aspect must be a dot, not an underscore.
**Fix:** Rename to `StudentService.Validations.cs`.
