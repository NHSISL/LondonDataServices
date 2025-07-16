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
    public partial class ResolvedAddressProcessingHealthCheckService : IResolvedAddressHealthItemService
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
                var failedResolvedAddressProcessingHealthCheckServiceException =
                    new FailedResolvedAddressProcessingHealthCheckServiceException(

                        message: "Failed resolved address processing health check service error occurred, "
                        + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressProcessingHealthCheckServiceException
                );
            }
        }

        private async ValueTask<ResolvedAddressProcessingHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressProcessingHealthCheckServiceException =
                new ResolvedAddressProcessingHealthCheckServiceException(

                    message: "Resolved address processing health check service error occurred, "
                    + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                resolvedAddressProcessingHealthCheckServiceException
            );

            return resolvedAddressProcessingHealthCheckServiceException;
        }
    }
}
