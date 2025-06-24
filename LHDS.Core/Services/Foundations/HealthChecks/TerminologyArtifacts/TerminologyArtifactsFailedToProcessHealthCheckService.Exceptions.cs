using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.TerminologyArtifacts
{
    public partial class TerminologyArtifactsFailedToProcessHealthCheckService : ITerminologyArtifactsHealthItemService
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
                var failedTerminologyArtifactsFailedToProcessHealthCheckServiceException =
                    new FailedTerminologyArtifactsFailedToProcessHealthCheckServiceException(

                        message: "Failed terminology artifacts failed to process health check service error occurred, "
                            + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedTerminologyArtifactsFailedToProcessHealthCheckServiceException
                );
            }
        }

        private async ValueTask<TerminologyArtifactsFailedToProcessHealthCheckServiceException> 
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var terminologyPollsFailedToProcessHealthCheckServiceException =
                new TerminologyArtifactsFailedToProcessHealthCheckServiceException(

                    message: "Terminology artifacts failed to process health check service error occurred, "
                        + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                terminologyPollsFailedToProcessHealthCheckServiceException
            );

            return terminologyPollsFailedToProcessHealthCheckServiceException;
        }
    }
}
