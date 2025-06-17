using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTrackings.FilesReceivedHealthCheck
{
    public partial class IngestionTrackingFilesReceivedHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingFilesReceivedHealthCheckServiceException =
                new FailedIngestionTrackingFilesReceivedHealthCheckServiceException(

                    message: "Failed ingestion tracking files received health check service error occurred, " +
                    "please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingFilesReceivedHealthCheckServiceException =
                new IngestionTrackingFilesReceivedHealthCheckServiceException(

                    message: "Ingestion tracking files received health check service error occurred, " +
                    "please contact support.",

                    innerException: failedIngestionTrackingFilesReceivedHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingFilesReceivedHealthCheckService.GetHealthStatusAsync();

            IngestionTrackingFilesReceivedHealthCheckServiceException
                actualIngestionTrackingFilesReceivedHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingFilesReceivedHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingFilesReceivedHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingFilesReceivedHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingFilesReceivedHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}