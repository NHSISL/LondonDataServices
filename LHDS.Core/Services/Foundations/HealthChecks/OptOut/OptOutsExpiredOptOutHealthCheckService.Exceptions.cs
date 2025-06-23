using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.OptOut
{
    public partial class OptOutsExpiredOptOutHealthCheckService : IOptOutHealthItemService
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
                var failedOptOutsExpiredOptOutsHealthCheckServiceException =
                    new FailedOptOutsExpiredOptOutsHealthCheckServiceException(

                        message: "Failed opt outs expired opt outs health check service error occurred, " +
                        "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedOptOutsExpiredOptOutsHealthCheckServiceException);
            }
        }

        private async ValueTask<OptOutsExpiredOptOutsHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var optOutsExpiredOptOutsHealthCheckServiceException =
                new OptOutsExpiredOptOutsHealthCheckServiceException(

                    message: "Opt outs expired opt outs health check service error occurred, " +
                    "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(optOutsExpiredOptOutsHealthCheckServiceException);

            return optOutsExpiredOptOutsHealthCheckServiceException;
        }
    }
}
