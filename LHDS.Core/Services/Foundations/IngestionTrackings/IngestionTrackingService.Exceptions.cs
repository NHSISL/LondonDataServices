using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
    }
}