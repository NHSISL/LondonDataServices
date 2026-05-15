// ---
// skill: the-standard-csharp-methods
// type: example
// source-section: "4. Methods — spacing, parameter breaking, chaining"
// demonstrates: "tsc-csharp-methods-001, tsc-csharp-methods-003, tsc-csharp-methods-004, tsc-csharp-methods-006, tsc-csharp-methods-007"
// ---

// ✅ GOOD: Blank line before return, named params, broken chain, stacked calls without gaps.

public async ValueTask<Student> AddStudentAsync(Student student)
{
    // validate
    ValidateStudent(student);

    // act
    Student addedStudent = await this.storageBroker.InsertStudentAsync(student);

    // ✅ blank line before return (tsc-csharp-methods-001)
    return addedStudent;
}

// ✅ Named parameter for literal (tsc-csharp-methods-004)
var student = new Student(name: "Josh", score: 150);

// ✅ Parameter breaking aligned to opening parenthesis (tsc-csharp-methods-003)
Student persistedStudent = await this.storageBroker.InsertStudentAsync(
    student: student,
    cancellationToken: cancellationToken);

// ✅ Stacked calls — no blank lines between them (tsc-csharp-methods-006)
ValidateStudentOnAdd(student);
ValidateStudentId(student.Id);
ValidateStudentName(student.Name);

// ✅ LINQ chain broken — one call per line (tsc-csharp-methods-007)
var activeStudentIds = students
    .Where(student => student.IsActive)
    .OrderBy(student => student.Name)
    .Select(student => student.Id)
    .ToList();
