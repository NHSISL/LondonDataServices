// ---
// skill: the-standard-csharp-variables
// type: example
// source-section: "5. Variables — naming, var usage, collections, null placeholder"
// demonstrates: "tsc-csharp-variables-001, tsc-csharp-variables-002, tsc-csharp-variables-004, tsc-csharp-variables-005, tsc-csharp-variables-008"
// ---

// ✅ GOOD: Full words, plural collections, correct var/explicit usage, maybe prefix.

// ✅ var when type is obvious (new expression)
var student = new Student
{
    Id = Guid.NewGuid(),
    Name = "Elbek"
};

// ✅ Explicit type when method name does not reveal return type
IQueryable<Student> allStudents = this.storageBroker.SelectAllStudents();

// ✅ Plural name for collection (tsc-csharp-variables-002)
IEnumerable<Student> activeStudents = allStudents.Where(student => student.IsActive);

// ✅ maybe prefix for nullable/uncertain result (tsc-csharp-variables-008)
Student maybeStudent = await this.storageBroker.SelectStudentByIdAsync(studentId);

// ✅ Long declaration broken to next line (tsc-csharp-variables-006)
ExternalEnrollmentResponse externalEnrollmentResponse =
    await this.enrollmentBroker.PostEnrollmentAsync(enrollmentRequest);
