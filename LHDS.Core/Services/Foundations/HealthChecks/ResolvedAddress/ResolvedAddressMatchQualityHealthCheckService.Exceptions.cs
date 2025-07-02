using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.ResolvedAddress
{
    public partial class ResolvedAddressMatchQualityHealthCheckService : IResolvedAddressHealthItemService
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
                var failedResolvedAddressMatchQualityHealthCheckServiceException =
                    new FailedResolvedAddressMatchQualityHealthCheckServiceException(

                        message: "Failed resolved address match quality health check service error occurred, "
                        + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressMatchQualityHealthCheckServiceException
                );
            }
        }

        private async ValueTask<ResolvedAddressMatchQualityHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressMatchQualityHealthCheckServiceException =
                new ResolvedAddressMatchQualityHealthCheckServiceException(

                    message: "Resolved address match quality health check service error occurred, "
                    + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                resolvedAddressMatchQualityHealthCheckServiceException
            );

            return resolvedAddressMatchQualityHealthCheckServiceException;
        }
    }
}
