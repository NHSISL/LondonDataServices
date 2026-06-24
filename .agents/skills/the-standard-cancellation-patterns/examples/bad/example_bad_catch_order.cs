// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "7.5 Whenever timeout logic exists, both catch blocks MUST exist"
// demonstrates: "tsc-csharp-cp-010 — plain OperationCanceledException catch placed before the timeout-guarded catch"
// ---

// ❌ The plain catch (OperationCanceledException) block appears BEFORE the
//    guarded catch block. The guarded block is unreachable because the plain
//    catch is a superset — it matches every OperationCanceledException,
//    including those triggered by the timeout source.
//    Timeouts will NEVER be classified as dependency failures.

public partial class StudentService
{
    private ValueTask<Student> TryCatch(
        ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return returningStudentFunction();
        }

        // ❌ Plain catch appears first — makes the guarded catch below unreachable.
        catch (OperationCanceledException)
        {
            throw;
        }

        // ❌ This block is UNREACHABLE — the plain catch above consumed every
        //    OperationCanceledException before the when-guard is ever evaluated.
        catch (OperationCanceledException)
            when (timeoutSource.IsCancellationRequested)
        {
            var timeoutException =
                new TimeoutException(
                    "The storage operation timed out.");

            throw CreateAndLogDependencyException(timeoutException);
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
