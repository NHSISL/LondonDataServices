// ---
// skill: the-standard-csharp-files
// type: example
// source-section: "0. Files — 0.0 Naming, 0.1 Partial Class Files"
// demonstrates: "tsc-csharp-files-001, tsc-csharp-files-002, tsc-csharp-files-003"
// ---

// ✅ GOOD: Correct PascalCase file names and correct partial class dot-notation.

// File: Student.cs
public class Student { }

// File: StudentService.cs
public partial class StudentService : IStudentService { }

// File: StudentService.Validations.cs
public partial class StudentService
{
    private void ValidateStudentOnAdd(Student student) { }
}

// File: StudentService.Validations.Add.cs
public partial class StudentService
{
    private void ValidateStudentId(Guid studentId) { }
}

// File: StudentService.Exceptions.cs
public partial class StudentService
{
    private delegate ValueTask<Student> ReturningStudentFunction();

    private async ValueTask<Student> TryCatch(ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return await returningStudentFunction();
        }
        catch (NullStudentException nullStudentException)
        {
            throw new StudentValidationException(nullStudentException);
        }
    }
}
