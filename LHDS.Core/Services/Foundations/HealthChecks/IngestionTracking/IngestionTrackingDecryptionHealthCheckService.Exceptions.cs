using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking
{
    public partial class IngestionTrackingDecryptionHealthCheckService : IIngestionTrackingHealthItemService
    {
        private delegate Task<HealthCheckResult> ReturningHealthCheckResultFunction();

        private async ValueTask<HealthCheckResult> TryCatch(ReturningHealthCheckResultFunction returningHealthCheckResultFunction)
        {
            try
            {
                return await returningHealthCheckResultFunction();
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingDecryptionHealthCheckCooridinationServiceException =
                    new FailedIngestionTrackingDecryptionHealthCheckCooridinationServiceException(
                        message: "Failed ingestion tracking decryption health check coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingDecryptionHealthCheckCooridinationServiceException);
            }
        }

        private async ValueTask<IngestionTrackingDecryptionHealthCheckCooridinationServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var ingestionTrackingDecryptionHealthCheckCooridinationServiceException =
                new IngestionTrackingDecryptionHealthCheckCooridinationServiceException(
                    message: "Ingestion tracking decryption health check coordination service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingDecryptionHealthCheckCooridinationServiceException);

            return ingestionTrackingDecryptionHealthCheckCooridinationServiceException;
        }
    }
}
