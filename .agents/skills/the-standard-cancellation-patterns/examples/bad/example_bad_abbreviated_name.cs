// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "3.4 Use the canonical parameter name"
// demonstrates: "tsc-csharp-cp-004 — parameter named ct or token instead of cancellationToken"
// ---

// ❌ CancellationToken parameter is named `ct` — an abbreviated non-canonical name.
//    Abbreviated names reduce readability and break codebase-wide consistency.

public interface IStudentService
{
    // ❌ Parameter named `ct` — must be `cancellationToken`
    ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken ct = default);
}

public class StudentService : IStudentService
{
    private readonly IStudentBroker studentBroker;

    public StudentService(IStudentBroker studentBroker) =>
        this.studentBroker = studentBroker;

    // ❌ Parameter named `ct`
    public ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken ct = default) =>
        TryCatch(async () =>
        {
            return await this.studentBroker.SelectStudentByIdAsync(
                studentId,
                ct);
        });
}

// Also forbidden:
//   CancellationToken token
//   CancellationToken cancelToken
