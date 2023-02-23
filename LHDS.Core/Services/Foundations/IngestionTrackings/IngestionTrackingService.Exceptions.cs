using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.IngestionTrackings;
using LHDS.Core.Models.IngestionTrackings.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private delegate ValueTask<IngestionTracking> ReturningIngestionTrackingFunction();

        private async ValueTask<IngestionTracking> TryCatch(ReturningIngestionTrackingFunction returningIngestionTrackingFunction)
        {
            try
            {
                return await returningIngestionTrackingFunction();
            }
            catch (NullIngestionTrackingException nullIngestionTrackingException)
            {
                throw CreateAndLogValidationException(nullIngestionTrackingException);
            }
            catch (InvalidIngestionTrackingException invalidIngestionTrackingException)
            {
                throw CreateAndLogValidationException(invalidIngestionTrackingException);
            }
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedIngestionTrackingStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsIngestionTrackingException =
                    new AlreadyExistsIngestionTrackingException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsIngestionTrackingException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidIngestionTrackingReferenceException =
                    new InvalidIngestionTrackingReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidIngestionTrackingReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedIngestionTrackingStorageException);
            }
        }

        private IngestionTrackingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ingestionTrackingValidationException =
                new IngestionTrackingValidationException(exception);

            this.loggingBroker.LogError(ingestionTrackingValidationException);

            return ingestionTrackingValidationException;
        }

        private IngestionTrackingDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ingestionTrackingDependencyException = new IngestionTrackingDependencyException(exception);
            this.loggingBroker.LogCritical(ingestionTrackingDependencyException);

            return ingestionTrackingDependencyException;
        }

        private IngestionTrackingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var ingestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(exception);

            this.loggingBroker.LogError(ingestionTrackingDependencyValidationException);

            return ingestionTrackingDependencyValidationException;
        }

        private IngestionTrackingDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var ingestionTrackingDependencyException = new IngestionTrackingDependencyException(exception);
            this.loggingBroker.LogError(ingestionTrackingDependencyException);

            return ingestionTrackingDependencyException;
        }
    }
}