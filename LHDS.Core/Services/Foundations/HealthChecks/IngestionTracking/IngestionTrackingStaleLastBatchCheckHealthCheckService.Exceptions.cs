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
    public partial class IngestionTrackingStaleLastBatchCheckHealthCheckService : IIngestionTrackingHealthItemService
    {
        private delegate Task<HealthCheckResult> ReturningHealthCheckResultFunction();

        private async ValueTask<HealthCheckResult> TryCatch(
            ReturningHealthCheckResultFunction returningHealthCheckResultFunction)
        {
            try
            {
                return await returningHealthCheckResultFunction();
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException =
                    new FailedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException(

                        message: "Failed ingestion tracking stale last batch check health check service " +
                            "error occurred, please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException);
            }
        }

        private async ValueTask<IngestionTrackingStaleLastBatchCheckHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingestionTrackingStaleLastBatchCheckHealthCheckServiceException =
                new IngestionTrackingStaleLastBatchCheckHealthCheckServiceException(

                    message: "Ingestion tracking stale last batch check health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingStaleLastBatchCheckHealthCheckServiceException);

            return ingestionTrackingStaleLastBatchCheckHealthCheckServiceException;
        }
    }
}
