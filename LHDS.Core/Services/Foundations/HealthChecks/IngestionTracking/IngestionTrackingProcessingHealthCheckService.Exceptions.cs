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
    public partial class IngestionTrackingProcessingHealthCheckService : IIngestionTrackingHealthItemService
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
                var failedIngestionTrackingProcessingHealthCheckServiceException =
                    new FailedIngestionTrackingProcessingHealthCheckServiceException(

                        message: "Failed ingestion tracking processing health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedIngestionTrackingProcessingHealthCheckServiceException
                );
            }
        }

        private async ValueTask<IngestionTrackingProcessingHealthCheckServiceException> 
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingestionTrackingProcessingHealthCheckServiceException =
                new IngestionTrackingProcessingHealthCheckServiceException(

                    message: "Ingestion tracking processing health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingProcessingHealthCheckServiceException);

            return ingestionTrackingProcessingHealthCheckServiceException;
        }
    }
}
