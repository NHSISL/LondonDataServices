// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "3.3 Never use nullable CancellationToken"
// demonstrates: "tsc-csharp-cp-003 — nullable CancellationToken? parameter"
// ---

// ❌ CancellationToken is declared as nullable (CancellationToken?).
//    This forces null-checks at every call site and breaks propagation contracts.

public interface IStudentService
{
    // ❌ Nullable CancellationToken? — must use CancellationToken cancellationToken = default
    ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken? cancellationToken);
}

public class StudentService : IStudentService
{
    private readonly IStudentBroker studentBroker;

    public StudentService(IStudentBroker studentBroker) =>
        this.studentBroker = studentBroker;

    // ❌ Accepts nullable token — requires null-check before forwarding
    public async ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken? cancellationToken)
    {
        CancellationToken token =
            cancellationToken ?? CancellationToken.None;

        return await this.studentBroker.SelectStudentByIdAsync(
            studentId,
            token);
    }
}
