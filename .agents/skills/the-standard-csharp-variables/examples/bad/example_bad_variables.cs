// ---
// skill: the-standard-csharp-variables
// type: example
// source-section: "5. Variables — naming, var usage, collections, null placeholder"
// demonstrates: "tsc-csharp-variables-001, tsc-csharp-variables-002, tsc-csharp-variables-003, tsc-csharp-variables-004, tsc-csharp-variables-008, tsc-csharp-variables-009"
// ---

// ❌ BAD: Multiple variable violations.

// ❌ Abbreviation — violates tsc-csharp-variables-001
var std = new Student { Id = Guid.NewGuid(), Name = "Elbek" };

// ❌ Type in name — violates tsc-csharp-variables-003
List<Student> studentList = new List<Student>();

// ❌ Type in name (singular) — violates tsc-csharp-variables-003
Student studentObj = await this.storageBroker.SelectStudentByIdAsync(id);

// ❌ var when type is not obvious — violates tsc-csharp-variables-004 / 005
var result = this.storageBroker.SelectAllStudents();

// ❌ Null placeholder without maybe prefix — violates tsc-csharp-variables-008
Student nullStudent = await this.storageBroker.SelectStudentByIdAsync(id);

// ❌ Multiple declarations on one line — violates tsc-csharp-variables-009
int count = 0, total = 0;
