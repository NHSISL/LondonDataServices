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
    public partial class ResolvedAddressFailedToExportHealthCheckService : IResolvedAddressHealthItemService
    {
        private delegate Task<HealthCheckResult> ReturningHealthCheckResultFunction();

        private async ValueTask<HealthCheckResult> TryCatch(ReturningHealthCheckResultFunction returningHealthCheckResultFunction)
        {
            try
            {
                return await returningHealthCheckResultFunction();
            }
            catch (Exception exception)
            {
                var failedResolvedAddressFailedToExportHealthCheckServiceException =
                    new FailedResolvedAddressFailedToExportHealthCheckServiceException(

                        message: "Failed resolved address failed to export health check service error occurred, "
                        + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressFailedToExportHealthCheckServiceException
                );
            }
        }

        private async ValueTask<ResolvedAddressFailedToExportHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressFailedToExportHealthCheckServiceException =
                new ResolvedAddressFailedToExportHealthCheckServiceException(

                    message: "Resolved address failed to export health check service error occurred, "
                    + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                resolvedAddressFailedToExportHealthCheckServiceException
            );

            return resolvedAddressFailedToExportHealthCheckServiceException;
        }
    }
}
