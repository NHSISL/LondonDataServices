using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using LHDS.Core.Models.Foundations.HealthChecks.ResolvedAddresses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.ResolvedAddress
{
    public partial class ResolvedAddressMatchingProcessHealthCheckService : IResolvedAddressHealthItemService
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
                var failedResolvedAddressMatchingProcessHealthCheckServiceException =
                    new FailedResolvedAddressMatchingProcessHealthCheckServiceException(

                        message: "Failed resolved address matching process health check service error occurred, "
                        + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedResolvedAddressMatchingProcessHealthCheckServiceException);
            }
        }

        private async ValueTask<ResolvedAddressMatchingProcessHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressMatchingProcessHealthCheckServiceException =
                new ResolvedAddressMatchingProcessHealthCheckServiceException(

                    message: "Resolved address matching process health check service error occurred, "
                    + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                resolvedAddressMatchingProcessHealthCheckServiceException);

            return resolvedAddressMatchingProcessHealthCheckServiceException;
        }
    }
}
