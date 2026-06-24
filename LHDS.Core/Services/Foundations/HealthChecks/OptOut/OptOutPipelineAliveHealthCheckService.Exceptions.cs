// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.OptOut
{
    public partial class OptOutPipelineAliveHealthCheckService : IOptOutHealthItemService
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
                var failedOptOutPipelineAliveHealthCheckServiceException =
                    new FailedOptOutPipelineAliveHealthCheckServiceException(

                        message: "Failed opt out pipeline alive health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedOptOutPipelineAliveHealthCheckServiceException);
            }
        }

        private async ValueTask<OptOutPipelineAliveHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var optOutPipelineAliveHealthCheckServiceException =
                new OptOutPipelineAliveHealthCheckServiceException(

                    message: "Opt out pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(optOutPipelineAliveHealthCheckServiceException);

            return optOutPipelineAliveHealthCheckServiceException;
        }
    }
}
