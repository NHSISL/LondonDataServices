using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTracking.FailedToProcessHealthCheck
{
    public partial class IngestionTrackingFailedToProcessHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingFailedToProcessHealthCheckServiceException =
                new FailedIngestionTrackingFailedToProcessHealthCheckServiceException(
                    message: "Failed ingestion tracking failed to process health check service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingFailedToProcessHealthCheckServiceException =
                new IngestionTrackingFailedToProcessHealthCheckServiceException(
                    message: "Ingestion tracking failed to process health check service error occurred, please contact support.",
                    innerException: failedIngestionTrackingFailedToProcessHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask = this.ingestionTrackingFailedToProcessHealthCheckService.GetHealthStatusAsync();

            IngestionTrackingFailedToProcessHealthCheckServiceException
                actualIngestionTrackingFailedToProcessHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingFailedToProcessHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingFailedToProcessHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingFailedToProcessHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingFailedToProcessHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
