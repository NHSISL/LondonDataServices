// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "7.4 If no timeout exists, do NOT catch OperationCanceledException"
// demonstrates: "tsc-csharp-cp-013 — unnecessary catch (OperationCanceledException) { throw; } with no timeout"
// ---

// ❌ A catch (OperationCanceledException) { throw; } block is present even though
//    there is no timeout source. This block adds noise, implies timeout logic is
//    present when it is not, and is entirely redundant — OperationCanceledException
//    already propagates naturally without a catch block.

public partial class StudentService
{
    public ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
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

        // ❌ Unnecessary — no timeoutSource exists; remove this block entirely.
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
}
