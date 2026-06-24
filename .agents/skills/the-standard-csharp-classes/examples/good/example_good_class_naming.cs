// ---
// skill: the-standard-csharp-classes
// type: example
// source-section: "2. Classes and Interfaces — 4.0 Naming, 4.1 Fields, 4.1.1 Referencing"
// demonstrates: "tsc-csharp-classes-001, tsc-csharp-classes-002, tsc-csharp-classes-005, tsc-csharp-classes-006"
// ---

// ✅ GOOD: Correct class names, camelCase fields, this. referencing.

// Model — singular, no type suffix
public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

// Service — singular, Service suffix
public class StudentService : IStudentService
{
    private readonly IStorageBroker storageBroker; // ✅ camelCase, no underscore

    public StudentService(IStorageBroker storageBroker)
    {
        this.storageBroker = storageBroker; // ✅ this. keyword used
    }
}

// Broker — singular, Broker suffix
public class StudentBroker : IStudentBroker { }

// Controller — PLURAL, Controller suffix
public class StudentsController : ControllerBase { }
