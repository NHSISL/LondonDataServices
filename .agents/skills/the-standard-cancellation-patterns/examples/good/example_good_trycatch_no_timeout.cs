// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "6.4 Methods using CancellationToken MUST validate cancellation / 7.4 If no timeout exists, do NOT catch OperationCanceledException"
// demonstrates: "tsc-csharp-cp-017, tsc-csharp-cp-013 — ThrowIfCancellationRequested called before dependency; no unnecessary catch block"
// ---

// ✅ ThrowIfCancellationRequested() is called before the dependency call — fail-fast.
//    No timeout differentiation is required.
//    No catch (OperationCanceledException) block is present.
//    OperationCanceledException propagates naturally through TryCatch.

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

    private ValueTask<Student> TryCatch(
        ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return returningStudentFunction();
        }
        catch (SqlException sqlException)
        {
            var failedStorageBrokerException =
                new FailedStorageBrokerException(
                    message: "Failed storage broker error occurred.",
                    innerException: sqlException);

            throw CreateAndLogDependencyException(failedStorageBrokerException);
        }
        catch (Exception exception)
        {
            var failedServiceException =
                new FailedStudentServiceException(
                    message: "Failed student service error occurred.",
                    innerException: exception);

            throw CreateAndLogServiceException(failedServiceException);
        }
        // NOTE: No catch (OperationCanceledException) — correct when no timeout exists.
    }
}
