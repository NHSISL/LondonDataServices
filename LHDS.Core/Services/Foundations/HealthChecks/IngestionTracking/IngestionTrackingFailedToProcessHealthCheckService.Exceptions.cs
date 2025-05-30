// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking
{
    public partial class IngestionTrackingFailedToProcessHealthCheckService : IIngestionTrackingHealthItemService
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
                var failedIngestionTrackingFailedToProcessHealthCheckServiceException =
                    new FailedIngestionTrackingFailedToProcessHealthCheckServiceException(
                        message: "Failed ingestion tracking failed to process health check service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingFailedToProcessHealthCheckServiceException);
            }
        }

        private async ValueTask<IngestionTrackingFailedToProcessHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var ingestionTrackingFailedToProcessHealthCheckServiceException =
                new IngestionTrackingFailedToProcessHealthCheckServiceException(
                    message: "Ingestion tracking failed to process health check service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingFailedToProcessHealthCheckServiceException);

            return ingestionTrackingFailedToProcessHealthCheckServiceException;
        }
    }
}
