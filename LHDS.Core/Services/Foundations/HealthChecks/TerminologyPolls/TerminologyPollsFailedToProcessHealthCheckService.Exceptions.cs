using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.TerminologyPolls
{
    public partial class TerminologyPollsFailedToProcessHealthCheckService : ITerminologyPollsHealthItemService
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
                var failedTerminologyPollsFailedToProcessHealthCheckServiceException =
                    new FailedTerminologyPollsFailedToProcessHealthCheckServiceException(

                        message: "Failed terminology polls failed to process health check service error occurred, "
                            + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedTerminologyPollsFailedToProcessHealthCheckServiceException
                );
            }
        }

        private async ValueTask<TerminologyPollsFailedToProcessHealthCheckServiceException> 
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var terminologyPollsFailedToProcessHealthCheckServiceException =
                new TerminologyPollsFailedToProcessHealthCheckServiceException(

                    message: "Terminology polls failed to process health check service error occurred, "
                        + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                terminologyPollsFailedToProcessHealthCheckServiceException
            );

            return terminologyPollsFailedToProcessHealthCheckServiceException;
        }
    }
}
