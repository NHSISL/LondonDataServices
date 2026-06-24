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
    public partial class ResolvedAddressQueuedHealthCheckService : IResolvedAddressHealthItemService
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
                var failedResolvedAddressQueuedHealthCheckServiceException =
                    new FailedResolvedAddressQueuedHealthCheckServiceException(

                        message: "Failed resolved address queued health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressQueuedHealthCheckServiceException);
            }
        }

        private async ValueTask<ResolvedAddressQueuedHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var resolvedAddressQueuedHealthCheckServiceException =
                new ResolvedAddressQueuedHealthCheckServiceException(

                    message: "Resolved address queued health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(resolvedAddressQueuedHealthCheckServiceException);

            return resolvedAddressQueuedHealthCheckServiceException;
        }
    }
}
