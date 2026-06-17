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
    public partial class IngestionTrackingLastSeenHealthCheckService : IIngestionTrackingHealthItemService
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
                var failedIngestionTrackingLastSeenHealthCheckServiceException =
                    new FailedIngestionTrackingLastSeenHealthCheckServiceException(

                        message: "Failed ingestion tracking last seen health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedIngestionTrackingLastSeenHealthCheckServiceException);
            }
        }

        private async ValueTask<IngestionTrackingLastSeenHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingestionTrackingLastSeenHealthCheckServiceException =
                new IngestionTrackingLastSeenHealthCheckServiceException(

                    message: "Ingestion tracking last seen health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingLastSeenHealthCheckServiceException);

            return ingestionTrackingLastSeenHealthCheckServiceException;
        }
    }
}
