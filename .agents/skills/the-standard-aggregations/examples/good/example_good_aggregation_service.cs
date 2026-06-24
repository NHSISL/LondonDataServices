// ---
// skill: the-standard-aggregations
// type: example
// source-section: "2.4 Aggregation Services"
// demonstrates: "ts-aggregations-001, ts-aggregations-002, ts-aggregations-003, ts-aggregations-004"
// ---

// ✅ GOOD: Aggregation service fans out to two orchestration services with no business logic.

public partial class StudentEnrollmentAggregationService : IStudentEnrollmentAggregationService
{
    private readonly IStudentLibraryOrchestrationService studentLibraryOrchestrationService;
    private readonly IStudentCourseOrchestrationService studentCourseOrchestrationService;
    private readonly ILoggingBroker loggingBroker;

    public StudentEnrollmentAggregationService(
        IStudentLibraryOrchestrationService studentLibraryOrchestrationService,
        IStudentCourseOrchestrationService studentCourseOrchestrationService,
        ILoggingBroker loggingBroker)
    {
        this.studentLibraryOrchestrationService = studentLibraryOrchestrationService;
        this.studentCourseOrchestrationService = studentCourseOrchestrationService;
        this.loggingBroker = loggingBroker;
    }

    public ValueTask EnrollStudentAsync(Student student) =>
        TryCatch(async () =>
        {
            await this.studentLibraryOrchestrationService
                .RegisterStudentWithLibraryCardAsync(student);

            await this.studentCourseOrchestrationService
                .RegisterStudentWithCoursesAsync(student);
        });
}
