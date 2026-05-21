// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.ResolvedAddress
{
    public partial class ResolvedAddressPipelineAliveHealthCheckService : IResolvedAddressHealthItemService
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
                var failedResolvedAddressPipelineAliveHealthCheckServiceException =
                    new FailedResolvedAddressPipelineAliveHealthCheckServiceException(

                        message: "Failed resolved address pipeline alive health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressPipelineAliveHealthCheckServiceException);
            }
        }

        private async ValueTask<ResolvedAddressPipelineAliveHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var resolvedAddressPipelineAliveHealthCheckServiceException =
                new ResolvedAddressPipelineAliveHealthCheckServiceException(

                    message: "Resolved address pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(resolvedAddressPipelineAliveHealthCheckServiceException);

            return resolvedAddressPipelineAliveHealthCheckServiceException;
        }
    }
}
