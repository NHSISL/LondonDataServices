// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings.RetryWarning
{
    public partial class IngestionTrackingRetryWarningHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingRetryWarningHealthCheckServiceException =
                new FailedIngestionTrackingRetryWarningHealthCheckServiceException(

                    message:
                        "Failed ingestion tracking retry warning health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingRetryWarningHealthCheckServiceException =
                new IngestionTrackingRetryWarningHealthCheckServiceException(

                    message:
                        "Ingestion tracking retry warning health check service error occurred, " +
                        "please contact support.",

                    innerException: failedIngestionTrackingRetryWarningHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            IngestionTrackingRetryWarningHealthCheckServiceException
                actualIngestionTrackingRetryWarningHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingRetryWarningHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingRetryWarningHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingRetryWarningHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingRetryWarningHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
