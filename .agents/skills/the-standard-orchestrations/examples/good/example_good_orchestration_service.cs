// ---
// skill: the-standard-orchestrations
// type: example
// source-section: "2.3 Orchestration Services"
// demonstrates: "ts-orchestrations-001, ts-orchestrations-002, ts-orchestrations-003"
// ---

// ✅ GOOD: Orchestration service coordinates StudentService and LibraryCardService.
// No broker dependency.

public partial class StudentLibraryOrchestrationService : IStudentLibraryOrchestrationService
{
    private readonly IStudentProcessingService studentProcessingService;
    private readonly ILibraryCardService libraryCardService;
    private readonly ILoggingBroker loggingBroker;

    public StudentLibraryOrchestrationService(
        IStudentProcessingService studentProcessingService,
        ILibraryCardService libraryCardService,
        ILoggingBroker loggingBroker)
    {
        this.studentProcessingService = studentProcessingService;
        this.libraryCardService = libraryCardService;
        this.loggingBroker = loggingBroker;
    }

    public ValueTask<Student> RegisterStudentWithLibraryCardAsync(Student student) =>
        TryCatch(async () =>
        {
            ValidateStudent(student);

            Student upsertedStudent =
                await this.studentProcessingService.UpsertStudentAsync(student);

            await this.libraryCardService.AddLibraryCardAsync(
                new LibraryCard { StudentId = upsertedStudent.Id });

            return upsertedStudent;
        });
}
