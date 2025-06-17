using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking
{
    public partial class IngestionTrackingFilesReceivedHealthCheckService : IIngestionTrackingHealthItemService
    {
        private delegate Task<HealthCheckResult> ReturningHealthCheckResultFunction();

        private async ValueTask<HealthCheckResult> TryCatch(
            ReturningHealthCheckResultFunction returningHealthCheckResultFunction
        )
        {
            try
            {
                return await returningHealthCheckResultFunction();
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingFilesReceivedHealthCheckServiceException =
                    new FailedIngestionTrackingFilesReceivedHealthCheckServiceException(

                        message: "Failed ingestion tracking files received health check service error occurred, " +
                        "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedIngestionTrackingFilesReceivedHealthCheckServiceException
                );
            }
        }

        private async ValueTask<IngestionTrackingFilesReceivedHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var ingestionTrackingFilesReceivedHealthCheckServiceException =
                new IngestionTrackingFilesReceivedHealthCheckServiceException(

                    message: "Ingestion tracking files received health check service error occurred, " +
                    "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingFilesReceivedHealthCheckServiceException);

            return ingestionTrackingFilesReceivedHealthCheckServiceException;
        }
    }
}
