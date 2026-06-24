// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "6.3 Link tokens for timeout support / 6.4 Methods using CancellationToken MUST validate cancellation / 7.0 Exception Handling Rules"
// demonstrates: "tsc-csharp-cp-009, tsc-csharp-cp-010, tsc-csharp-cp-011, tsc-csharp-cp-012, tsc-csharp-cp-017 — ThrowIfCancellationRequested, linked timeout source and correct catch ordering"
// ---

// ✅ ThrowIfCancellationRequested() is called before the dependency call — fail-fast.
//    Timeout source is linked to the upstream token.
//    The guarded catch (when timeoutSource.IsCancellationRequested) precedes
//    the plain catch (OperationCanceledException).
//    TimeoutException is wrapped as a dependency failure.
//    OperationCanceledException is rethrown with throw;.

public partial class StudentService
{
    private ValueTask<Student> TryCatch(
        ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return returningStudentFunction();
        }
        catch (OperationCanceledException)
            when (timeoutSource.IsCancellationRequested)
        {
            var timeoutException =
                new TimeoutException(
                    "The storage operation timed out.");

            throw CreateAndLogDependencyException(timeoutException);
        }
        catch (OperationCanceledException)
        {
            throw;
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
    }

    // Caller sets up the linked source before invoking TryCatch.
    public async ValueTask<Student> RetrieveStudentByIdWithTimeoutAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var timeoutSource =
            new CancellationTokenSource(
                TimeSpan.FromSeconds(30));

        using var linkedSource =
            CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                timeoutSource.Token);

        return await this.studentBroker.SelectStudentByIdAsync(
            studentId,
            linkedSource.Token);
    }
}
