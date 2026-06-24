// ---
// skill: the-standard-csharp-methods
// type: example
// source-section: "4. Methods — spacing, parameter breaking, chaining"
// demonstrates: "tsc-csharp-methods-001, tsc-csharp-methods-002, tsc-csharp-methods-007, tsc-csharp-methods-008, tsc-csharp-methods-009"
// ---

// ❌ BAD: Multiple method violations.

public async ValueTask<Student> AddStudentAsync(Student student)
{
    ValidateStudent(student);
    Student addedStudent = await this.storageBroker.InsertStudentAsync(student);
    return addedStudent; // ❌ missing blank line before return — violates tsc-csharp-methods-001
}

// ❌ Abbreviated method name — violates tsc-csharp-methods-009
public decimal CalcTtl(IEnumerable<Student> studs) => studs.Sum(s => s.Score);

// ❌ Line too long (exceeds 120 chars) — violates tsc-csharp-methods-002
Student persistedStudent = await this.storageBroker.InsertStudentAsync(student: student, cancellationToken: cancellationToken);

// ❌ LINQ chain on a single line (>120 chars) — violates tsc-csharp-methods-007
var ids = students.Where(s => s.IsActive).OrderBy(s => s.Name).Select(s => s.Id).ToList();

// ❌ Mixed chaining — one call broken, others inline — violates tsc-csharp-methods-008
var mixed = students.Where(s =>
    s.IsActive).OrderBy(s => s.Name).Select(s => s.Id).ToList();
