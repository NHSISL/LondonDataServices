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
    public partial class ResolvedAddressExportHealthCheckService : IResolvedAddressHealthItemService
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
                var failedResolvedAddressExportHealthCheckServiceException =
                    new FailedResolvedAddressExportHealthCheckServiceException(

                        message: "Failed resolved address export health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressExportHealthCheckServiceException);
            }
        }

        private async ValueTask<ResolvedAddressExportHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var resolvedAddressExportHealthCheckServiceException =
                new ResolvedAddressExportHealthCheckServiceException(

                    message: "Resolved address export health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(resolvedAddressExportHealthCheckServiceException);

            return resolvedAddressExportHealthCheckServiceException;
        }
    }
}
