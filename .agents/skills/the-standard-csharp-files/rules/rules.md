# C# Coding Standard — Files — Rules

## FILE NAMING

**tsc-csharp-files-001** [ERROR] Every `.cs` file must be named using PascalCase followed by the `.cs` extension (e.g., `Student.cs`, `StudentService.cs`).
**tsc-csharp-files-002** [ERROR] Partial class files must be named `{RootClass}.{Aspect}.cs` using a dot separator (e.g., `StudentService.Validations.cs`, `StudentService.Exceptions.cs`).
**tsc-csharp-files-003** [ERROR] Partial class files with a sub-aspect must be named `{RootClass}.{Aspect}.{SubAspect}.cs` (e.g., `StudentService.Validations.Add.cs`).
**tsc-csharp-files-004** [ERROR] File names must not use camelCase, all-lowercase, underscores, or hyphens as word separators.
**tsc-csharp-files-005** [ERROR] Partial class file names must not concatenate aspects without a dot separator (e.g., `StudentServiceValidations.cs` is forbidden).
