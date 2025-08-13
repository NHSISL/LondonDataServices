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
    public partial class IngestionTrackingIncompleteBatchHealthCheckService : IIngestionTrackingHealthItemService
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
                var failedIngestionTrackingIncompleteBatchHealthCheckServiceException =
                    new FailedIngestionTrackingIncompleteBatchHealthCheckServiceException(

                        message: "Failed ingestion tracking incomplete batch health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedIngestionTrackingIncompleteBatchHealthCheckServiceException);
            }
        }

        private async ValueTask<IngestionTrackingIncompleteBatchHealthCheckServiceException> 
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingestionTrackingIncompleteBatchHealthCheckServiceException =
                new IngestionTrackingIncompleteBatchHealthCheckServiceException(

                    message: "Ingestion tracking incomplete batch health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingIncompleteBatchHealthCheckServiceException);

            return ingestionTrackingIncompleteBatchHealthCheckServiceException;
        }
    }
}
