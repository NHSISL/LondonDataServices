// ---
// skill: the-standard-exceptions
// type: example
// source-section: "2.1.3.1 Exception Models"
// demonstrates: "ts-exceptions-002, ts-exceptions-007 — naked infrastructure exception and generic name"
// ---

// ❌ BAD: No entity qualifier; infrastructure exception propagates unwrapped; raw Exception base class.

// ❌ Generic names with no entity qualifier — violates ts-exceptions-007
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

public class ServiceException : Exception
{
    public ServiceException(string message) : base(message) { }
}

// ❌ Usage in service — SqlException propagates naked, raw Exception thrown
public async ValueTask<Student> AddStudentAsync(Student student)
{
    if (student is null)
        throw new ValidationException("Student is null."); // ❌ generic, not Xeption-based

    try
    {
        return await this.storageBroker.InsertStudentAsync(student);
    }
    catch (SqlException)
    {
        // ❌ SqlException allowed to propagate naked — violates ts-exceptions-002
        throw;
    }
    catch (Exception exception)
    {
        // ❌ raw Exception base class — violates ts-exceptions-005
        throw new Exception("Something went wrong.", exception);
    }
}
