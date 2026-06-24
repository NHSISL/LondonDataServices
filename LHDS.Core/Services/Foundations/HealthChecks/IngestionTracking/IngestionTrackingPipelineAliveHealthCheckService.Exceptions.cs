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
    public partial class IngestionTrackingPipelineAliveHealthCheckService : IIngestionTrackingHealthItemService
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
                var failedIngestionTrackingPipelineAliveHealthCheckServiceException =
                    new FailedIngestionTrackingPipelineAliveHealthCheckServiceException(

                        message: "Failed ingestion tracking pipeline alive health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedIngestionTrackingPipelineAliveHealthCheckServiceException);
            }
        }

        private async ValueTask<IngestionTrackingPipelineAliveHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingestionTrackingPipelineAliveHealthCheckServiceException =
                new IngestionTrackingPipelineAliveHealthCheckServiceException(

                    message: "Ingestion tracking pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingPipelineAliveHealthCheckServiceException);

            return ingestionTrackingPipelineAliveHealthCheckServiceException;
        }
    }
}
