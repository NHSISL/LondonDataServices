// ---
// skill: the-standard-aggregations
// type: example
// source-section: "2.4 Aggregation Services"
// demonstrates: "ts-aggregations-004 — aggregation service containing business logic and data transformation"
// ---

// ❌ BAD: Aggregation service enforces a business rule and transforms data.

public class StudentEnrollmentAggregationService : IStudentEnrollmentAggregationService
{
    private readonly IStudentLibraryOrchestrationService studentLibraryOrchestrationService;
    private readonly IStudentCourseOrchestrationService studentCourseOrchestrationService;
    private readonly IStudentService studentService; // ❌ foundation dependency

    public async ValueTask<EnrollmentSummary> EnrollStudentAsync(Student student)
    {
        // ❌ business rule in aggregation layer
        Student existingStudent =
            await this.studentService.RetrieveStudentByIdAsync(student.Id);

        if (existingStudent is not null)
            throw new InvalidOperationException("Student already enrolled.");

        Student libraryStudent =
            await this.studentLibraryOrchestrationService
                .RegisterStudentWithLibraryCardAsync(student);

        Student courseStudent =
            await this.studentCourseOrchestrationService
                .RegisterStudentWithCoursesAsync(student);

        // ❌ data transformation in aggregation layer
        return new EnrollmentSummary
        {
            StudentId = student.Id,
            HasLibraryCard = libraryStudent is not null,
            CourseCount = courseStudent.Courses.Count
        };
    }
}
