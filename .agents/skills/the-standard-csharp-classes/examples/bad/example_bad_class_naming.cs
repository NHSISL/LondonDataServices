// ---
// skill: the-standard-csharp-classes
// type: example
// source-section: "2. Classes and Interfaces — 4.0 Naming, 4.1 Fields"
// demonstrates: "tsc-csharp-classes-001, tsc-csharp-classes-002, tsc-csharp-classes-005, tsc-csharp-classes-006"
// ---

// ❌ BAD: Forbidden class names, underscore field, missing this., wrong initializer order.

// ❌ Model suffix — violates tsc-csharp-classes-001
public class StudentModel { }

// ❌ BL suffix — violates tsc-csharp-classes-002
public class StudentBL { }

// ❌ Plural service — violates tsc-csharp-classes-002
public class StudentsService { }

// ❌ Singular controller — violates tsc-csharp-classes-004
public class StudentController { }

// ❌ Underscore field prefix — violates tsc-csharp-classes-005
public class BadStudentService
{
    private readonly string _studentName; // ❌ must be camelCase: studentName

    public BadStudentService(string studentName)
    {
        _studentName = studentName; // ❌ missing this. — violates tsc-csharp-classes-006
    }
}

// ❌ Object initializer order reversed — violates tsc-csharp-classes-008
// Class declares Id first, then Name — but initializer assigns Name first:
var student = new Student
{
    Name = "Elbek",  // ❌ should be second
    Id = Guid.NewGuid() // ❌ should be first
};

// ❌ Positional literals without aliases — violates tsc-csharp-classes-007
var student2 = new Student("Josh", 150);

// ❌ Target-typed new — violates tsc-csharp-classes-009
Student student3 = new (...);
