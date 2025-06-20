using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.PDS
{
    public partial class PdsReceivedReplyHealthCheckService : IPdsHealthItemService
    {
        private delegate Task<HealthCheckResult> ReturningHealthCheckResultFunction();

        private async ValueTask<HealthCheckResult> TryCatch(
            ReturningHealthCheckResultFunction returningHealthCheckResultFunction
        )
        {
            try
            {
                return await returningHealthCheckResultFunction();
            }
            catch (Exception exception)
            {
                var failedPdsReceivedReplyHealthCheckServiceException =
                    new FailedPdsReceivedReplyHealthCheckServiceException(

                        message: "Failed pds received reply health check service error occurred, " +
                            "please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedPdsReceivedReplyHealthCheckServiceException
                );
            }
        }

        private async ValueTask<PdsReceivedReplyHealthCheckServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var pdsReceivedReplyHealthCheckServiceException =
                new PdsReceivedReplyHealthCheckServiceException(

                    message: "Pds received reply health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(pdsReceivedReplyHealthCheckServiceException);

            return pdsReceivedReplyHealthCheckServiceException;
        }
    }
}
