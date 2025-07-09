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
    public partial class ResolvedAddressFailedToProcessHealthCheckService : IResolvedAddressHealthItemService
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
                var failedResolvedAddressFailedToProcessHealthCheckServiceException =
                    new FailedResolvedAddressFailedToProcessHealthCheckServiceException(

                        message: 
                            "Failed resolved address failed to process health check service error occurred, "
                            + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressFailedToProcessHealthCheckServiceException
                );
            }
        }

        private async ValueTask<ResolvedAddressFailedToProcessHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressFailedToProcessHealthCheckServiceException =
                new ResolvedAddressFailedToProcessHealthCheckServiceException(

                    message: 
                        "Resolved address failed to process health check service error occurred, "
                        + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                resolvedAddressFailedToProcessHealthCheckServiceException
            );

            return resolvedAddressFailedToProcessHealthCheckServiceException;
        }
    }
}
