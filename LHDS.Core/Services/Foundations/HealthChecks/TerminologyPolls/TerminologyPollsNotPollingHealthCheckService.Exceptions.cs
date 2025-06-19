using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.TerminologyPolls
{
    public partial class TerminologyPollsNotPollingHealthCheckService : ITerminologyPollsHealthItemService
    {
        private delegate Task<HealthCheckResult> ReturningHealthCheckResultFunction();

        private async ValueTask<HealthCheckResult> 
            TryCatch(ReturningHealthCheckResultFunction returningHealthCheckResultFunction)
        {
            try
            {
                return await returningHealthCheckResultFunction();
            }
            catch (Exception exception)
            {
                var failedTerminologyPollsNotPollingHealthCheckServiceException =
                    new FailedTerminologyPollsNotPollingHealthCheckServiceException(

                        message: "Failed terminology polls not polling health check service error occurred, "
                        + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedTerminologyPollsNotPollingHealthCheckServiceException
                );
            }
        }

        private async ValueTask<TerminologyPollsNotPollingHealthCheckServiceException> 
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var terminologyPollsNotPollingHealthCheckServiceException =
                new TerminologyPollsNotPollingHealthCheckServiceException(

                    message: "Terminology polls not polling health check service error occurred, "
                    + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                terminologyPollsNotPollingHealthCheckServiceException
            );

            return terminologyPollsNotPollingHealthCheckServiceException;
        }
    }
}
