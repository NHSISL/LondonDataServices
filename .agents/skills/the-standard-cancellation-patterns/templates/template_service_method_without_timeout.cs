// ---
// skill: the-standard-cancellation-patterns
// type: template
// source-section: "6.4 Methods using CancellationToken MUST validate cancellation / 7.4 If no timeout exists, do NOT catch OperationCanceledException"
// ---

// Foundation service method using TryCatch with CancellationToken only.
// ThrowIfCancellationRequested() is called for fail-fast behaviour.
// No timeout source is created. No catch (OperationCanceledException) block is present.
// OperationCanceledException propagates naturally.
//
// Replace all occurrences of:
//   {Entity}  → the entity name (e.g., Student)
//   {entity}  → camel-case entity name (e.g., student)

public partial class {Entity}Service
{
    public ValueTask<{Entity}> Retrieve{Entity}ByIdAsync(
        Guid {entity}Id,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await this.{entity}Broker.Select{Entity}ByIdAsync(
                {entity}Id,
                cancellationToken);
        });

    private ValueTask<{Entity}> TryCatch(
        Returning{Entity}Function returning{Entity}Function)
    {
        try
        {
            return returning{Entity}Function();
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
        // NOTE: No catch (OperationCanceledException) — correct when no timeout exists.
        //       OperationCanceledException propagates naturally through TryCatch.
    }
}
