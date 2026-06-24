// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "6.1 The same token MUST flow through all layers"
// demonstrates: "tsc-csharp-cp-007 — token not passed to the dependency call"
// ---

// ❌ The service accepts CancellationToken but silently drops it —
//    the broker call does not receive the token.
//    The consumer's cancellation signal cannot reach the I/O layer.

public partial class StudentService : IStudentService
{
    private readonly IStudentBroker studentBroker;

    public StudentService(IStudentBroker studentBroker) =>
        this.studentBroker = studentBroker;

    public ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            // ❌ cancellationToken is NOT forwarded to the broker call.
            return await this.studentBroker.SelectStudentByIdAsync(studentId);
        });
}
