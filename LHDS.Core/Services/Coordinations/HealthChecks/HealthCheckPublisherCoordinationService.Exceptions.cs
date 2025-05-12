// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.HealthChecks
{
    public partial class HealthCheckPublisherCoordinationService
    {
        private delegate Task ReturningNothingFunction();

        private async Task TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (Exception exception)
            {
                var failedHealthCheckPublisherServiceException =
                    new FailedHealthCheckPublisherCoordinationServiceException(
                        message: "Failed health check publisher coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedHealthCheckPublisherServiceException);
            }
        }

        private async ValueTask<HealthCheckPublisherCoordinationServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var healthCheckPublisherServiceException =
                new HealthCheckPublisherCoordinationServiceException(
                    message: "Health check publisher coordination service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(healthCheckPublisherServiceException);

            return healthCheckPublisherServiceException;
        }
    }
}
