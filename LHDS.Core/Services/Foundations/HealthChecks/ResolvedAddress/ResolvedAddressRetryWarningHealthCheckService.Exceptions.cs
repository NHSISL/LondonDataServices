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
    public partial class ResolvedAddressRetryWarningHealthCheckService : IResolvedAddressHealthItemService
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
                var failedResolvedAddressRetryWarningHealthCheckServiceException =
                    new FailedResolvedAddressRetryWarningHealthCheckServiceException(

                        message: "Failed resolved address retry warning health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressRetryWarningHealthCheckServiceException);
            }
        }

        private async ValueTask<ResolvedAddressRetryWarningHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var resolvedAddressRetryWarningHealthCheckServiceException =
                new ResolvedAddressRetryWarningHealthCheckServiceException(

                    message: "Resolved address retry warning health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(resolvedAddressRetryWarningHealthCheckServiceException);

            return resolvedAddressRetryWarningHealthCheckServiceException;
        }
    }
}
