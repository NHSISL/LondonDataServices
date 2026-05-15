// ---
// skill: the-standard-processings
// type: example
// source-section: "2.2 Processing Services"
// demonstrates: "ts-processings-001, ts-processings-002, ts-processings-003 — UpsertStudentAsync workflow"
// ---

// ✅ GOOD: Processing service combines Retrieve + Add/Modify into an Upsert workflow.
// No broker dependency — only foundation service.

public partial class StudentProcessingService : IStudentProcessingService
{
    private readonly IStudentService studentService;
    private readonly ILoggingBroker loggingBroker;

    public StudentProcessingService(
        IStudentService studentService,
        ILoggingBroker loggingBroker)
    {
        this.studentService = studentService;
        this.loggingBroker = loggingBroker;
    }

    public ValueTask<Student> UpsertStudentAsync(Student student) =>
        TryCatch(async () =>
        {
            ValidateStudentOnUpsert(student);

            IQueryable<Student> allStudents =
                await this.studentService.RetrieveAllStudentsAsync();

            bool studentExists = allStudents
                .Any(retrievedStudent => retrievedStudent.Id == student.Id);

            return studentExists switch
            {
                true => await this.studentService.ModifyStudentAsync(student),
                false => await this.studentService.AddStudentAsync(student)
            };
        });
}
