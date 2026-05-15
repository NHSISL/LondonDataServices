// ---
// skill: the-standard-cancellation-patterns
// type: template
// source-section: "6.3 Link tokens for timeout support / 6.4 Methods using CancellationToken MUST validate cancellation / 7.0 Exception Handling Rules"
// ---

// Foundation service method using TryCatch with a linked timeout source
// and the mandatory catch block ordering.
// ThrowIfCancellationRequested() is called for fail-fast behaviour.
//
// Replace all occurrences of:
//   {Entity}        → the entity name (e.g., Student)
//   {entity}        → camel-case entity name (e.g., student)
//   {timeoutSeconds} → timeout duration in seconds (e.g., 30)

public partial class {Entity}Service
{
    public ValueTask<{Entity}> Retrieve{Entity}ByIdAsync(
        Guid {entity}Id,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var timeoutSource =
                new CancellationTokenSource(
                    TimeSpan.FromSeconds({timeoutSeconds}));

            using var linkedSource =
                CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken,
                    timeoutSource.Token);

            return await this.{entity}Broker.Select{Entity}ByIdAsync(
                {entity}Id,
                linkedSource.Token);
        });

    private ValueTask<{Entity}> TryCatch(
        Returning{Entity}Function returning{Entity}Function)
    {
        try
        {
            return returning{Entity}Function();
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
            var failed{Entity}ServiceException =
                new Failed{Entity}ServiceException(
                    message: "Failed {entity} service error occurred.",
                    innerException: exception);

            throw CreateAndLogServiceException(failed{Entity}ServiceException);
        }
    }
}
