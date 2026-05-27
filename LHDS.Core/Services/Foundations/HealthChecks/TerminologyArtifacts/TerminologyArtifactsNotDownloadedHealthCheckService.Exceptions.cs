using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.TerminologyArtifacts
{
    public partial class TerminologyArtifactsNotDownloadedHealthCheckService : ITerminologyArtifactsHealthItemService
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
                var failedTerminologyArtifactsNotDownloadedHealthCheckServiceException =
                    new FailedTerminologyArtifactsNotDownloadedHealthCheckServiceException(

                        message: "Failed terminology artifacts not downloaded health check service error occurred, "
                            + "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedTerminologyArtifactsNotDownloadedHealthCheckServiceException);
            }
        }

        private async ValueTask<TerminologyArtifactsNotDownloadedHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var terminologyArtifactsNotDownloadedHealthCheckServiceException =
                new TerminologyArtifactsNotDownloadedHealthCheckServiceException(

                    message: "Terminology artifacts not downloaded health check service error occurred, "
                        + "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                terminologyArtifactsNotDownloadedHealthCheckServiceException);

            return terminologyArtifactsNotDownloadedHealthCheckServiceException;
        }
    }
}
