// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "7.3 OperationCanceledException MUST NEVER be wrapped"
// demonstrates: "tsc-csharp-cp-012 — OperationCanceledException wrapped in FailedServiceException"
// ---

// ❌ OperationCanceledException is caught and wrapped in FailedServiceException.
//    This destroys the cooperative cancellation contract. Callers (ASP.NET Core,
//    Polly, hosted services) can no longer detect a true cancellation.

public partial class StudentService
{
    private ValueTask<Student> TryCatch(
        ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return returningStudentFunction();
        }

        // ❌ Wrapping OperationCanceledException in a service exception
        catch (OperationCanceledException exception)
        {
            var failedServiceException =
                new FailedStudentServiceException(
                    message: "Failed student service error occurred.",
                    innerException: exception);

            throw CreateAndLogServiceException(failedServiceException);
        }

        // ❌ Also forbidden — wrapping in a dependency exception
        // catch (OperationCanceledException exception)
        // {
        //     var failedDependencyException =
        //         new FailedStudentDependencyException(
        //             message: "Failed student dependency error occurred.",
        //             innerException: exception);
        //
        //     throw CreateAndLogDependencyException(failedDependencyException);
        // }

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
