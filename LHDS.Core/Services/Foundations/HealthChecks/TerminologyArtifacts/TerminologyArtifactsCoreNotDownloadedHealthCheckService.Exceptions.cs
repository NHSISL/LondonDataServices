// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks.TerminologyArtifacts
{
    public partial class TerminologyArtifactsCoreNotDownloadedHealthCheckService : ITerminologyArtifactsHealthItemService
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
                var failedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException =
                    new FailedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException(

                        message: "Failed terminology artifacts core not downloaded health check service error " +
                            "occurred, please contact support.",

                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(
                    failedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException);
            }
        }

        private async ValueTask<TerminologyArtifactsCoreNotDownloadedHealthCheckServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var terminologyArtifactsCoreNotDownloadedHealthCheckServiceException =
                new TerminologyArtifactsCoreNotDownloadedHealthCheckServiceException(

                    message: "Terminology artifacts core not downloaded health check service error occurred, " +
                        "please contact support.",

                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(
                terminologyArtifactsCoreNotDownloadedHealthCheckServiceException);

            return terminologyArtifactsCoreNotDownloadedHealthCheckServiceException;
        }
    }
}
