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
    public partial class IngestionTrackingRetryWarningHealthCheckService : IIngestionTrackingHealthItemService
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
                var failedIngestionTrackingRetryWarningHealthCheckServiceException =
                    new FailedIngestionTrackingRetryWarningHealthCheckServiceException(

                        message: "Failed ingestion tracking retry warning health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedIngestionTrackingRetryWarningHealthCheckServiceException);
            }
        }

        private async ValueTask<IngestionTrackingRetryWarningHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingestionTrackingRetryWarningHealthCheckServiceException =
                new IngestionTrackingRetryWarningHealthCheckServiceException(

                    message: "Ingestion tracking retry warning health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingRetryWarningHealthCheckServiceException);

            return ingestionTrackingRetryWarningHealthCheckServiceException;
        }
    }
}
