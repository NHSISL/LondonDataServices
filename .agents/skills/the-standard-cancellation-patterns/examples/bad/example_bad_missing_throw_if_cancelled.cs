// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "6.4 Methods using CancellationToken MUST validate cancellation"
// demonstrates: "tsc-csharp-cp-017 — missing ThrowIfCancellationRequested before dependency call"
// ---

// ❌ cancellationToken.ThrowIfCancellationRequested() is not called.
//    If cancellation was already requested before this method was entered,
//    work proceeds unnecessarily and the dependency is called regardless.

public partial class StudentService
{
    public ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            // ❌ Missing: cancellationToken.ThrowIfCancellationRequested();

            return await this.studentBroker.SelectStudentByIdAsync(
                studentId,
                cancellationToken);
        });
}

// ✅ Correct — fail-fast check added before the dependency call.

public partial class StudentService
{
    public ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await this.studentBroker.SelectStudentByIdAsync(
                studentId,
                cancellationToken);
        });
}
