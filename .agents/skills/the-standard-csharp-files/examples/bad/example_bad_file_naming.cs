// ---
// skill: the-standard-csharp-files
// type: example
// source-section: "0. Files — 0.0 Naming, 0.1 Partial Class Files"
// demonstrates: "tsc-csharp-files-001, tsc-csharp-files-004, tsc-csharp-files-005"
// ---

// ❌ BAD: The following file names violate the C# Coding Standard naming rules.

// ❌ File named: student.cs          — violates tsc-csharp-files-001 (must be PascalCase: Student.cs)
// ❌ File named: studentService.cs   — violates tsc-csharp-files-001 (must be StudentService.cs)
// ❌ File named: Student_Service.cs  — violates tsc-csharp-files-004 (no underscores)
// ❌ File named: StudentServiceValidations.cs — violates tsc-csharp-files-005 (must be StudentService.Validations.cs)
// ❌ File named: StudentService_Validations.cs — violates tsc-csharp-files-002, tsc-csharp-files-004 (must use dot: StudentService.Validations.cs)

// This file represents what is WRONG — never name files this way.
public class Example { }
