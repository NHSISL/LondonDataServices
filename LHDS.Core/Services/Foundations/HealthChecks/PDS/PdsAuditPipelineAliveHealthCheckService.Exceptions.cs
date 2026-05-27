// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.PDS
{
    public partial class PdsAuditPipelineAliveHealthCheckService : IPdsHealthItemService
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
                var failedPdsAuditPipelineAliveHealthCheckServiceException =
                    new FailedPdsAuditPipelineAliveHealthCheckServiceException(

                        message: "Failed PDS audit pipeline alive health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedPdsAuditPipelineAliveHealthCheckServiceException);
            }
        }

        private async ValueTask<PdsAuditPipelineAliveHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var pdsAuditPipelineAliveHealthCheckServiceException =
                new PdsAuditPipelineAliveHealthCheckServiceException(

                    message: "PDS audit pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(pdsAuditPipelineAliveHealthCheckServiceException);

            return pdsAuditPipelineAliveHealthCheckServiceException;
        }
    }
}
