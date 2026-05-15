// ---
// skill: the-standard-exceptions
// type: example
// source-section: "2.1.3.1 Exception Models"
// demonstrates: "ts-exceptions-001, ts-exceptions-005, ts-exceptions-007 — full exception hierarchy for StudentService"
// ---

// ✅ GOOD: Complete three-category exception hierarchy; all inherit from Xeption; entity name in every class name.

// Specific inner exceptions
public class NullStudentException : Xeption
{
    public NullStudentException(Exception innerException)
        : base(message: "Student is null.", innerException) { }
}

public class InvalidStudentException : Xeption
{
    public InvalidStudentException()
        : base(message: "Invalid student, fix the errors and try again.") { }
}

public class NotFoundStudentException : Xeption
{
    public NotFoundStudentException(Guid studentId)
        : base(message: $"Could not find student with id: {studentId}.") { }
}

public class AlreadyExistsStudentException : Xeption
{
    public AlreadyExistsStudentException(Exception innerException)
        : base(message: "Student already exists.", innerException) { }
}

public class FailedStudentStorageException : Xeption
{
    public FailedStudentStorageException(Exception innerException)
        : base(message: "Failed student storage error occured, contact support.", innerException) { }
}

public class FailedStudentServiceException : Xeption
{
    public FailedStudentServiceException(Exception innerException)
        : base(message: "Failed student service error occured, contact support.", innerException) { }
}

// Category wrappers
public class StudentValidationException : Xeption
{
    public StudentValidationException(Xeption innerException)
        : base(message: "Student validation errors occured, please try again.", innerException) { }
}

public class StudentDependencyValidationException : Xeption
{
    public StudentDependencyValidationException(Xeption innerException)
        : base(message: "Student dependency validation error occured, fix the errors and try again.", innerException) { }
}

public class StudentDependencyException : Xeption
{
    public StudentDependencyException(Xeption innerException)
        : base(message: "Student dependency error occured, contact support.", innerException) { }
}

public class StudentServiceException : Xeption
{
    public StudentServiceException(Xeption innerException)
        : base(message: "Student service error occured, contact support.", innerException) { }
}
