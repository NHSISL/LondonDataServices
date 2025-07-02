using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.TerminologyArtifacts.FailedToProcessHealthCheck
{
    public partial class TerminologyArtifactsFailedToProcessHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedTerminologyArtifactsFailedToProcessHealthCheckServiceException =
                new FailedTerminologyArtifactsFailedToProcessHealthCheckServiceException(

                    message:
                        "Failed terminology artifacts failed to process health check service error occurred, "
                            + "please contact support.",

                    innerException: serviceException);

            var expectedTerminologyArtifactsFailedToProcessHealthCheckServiceException =
                new TerminologyArtifactsFailedToProcessHealthCheckServiceException(

                    message:
                        "Terminology artifacts failed to process health check service error occurred, "
                            + "please contact support.",

                    innerException: failedTerminologyArtifactsFailedToProcessHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask = 
                this.terminologyArtifactsFailedToProcessHealthCheckService.GetHealthStatusAsync();

            TerminologyArtifactsFailedToProcessHealthCheckServiceException
                actualTerminologyArtifactsFailedToProcessHealthCheckServiceException =
                    await Assert.ThrowsAsync<TerminologyArtifactsFailedToProcessHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualTerminologyArtifactsFailedToProcessHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactsFailedToProcessHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactsFailedToProcessHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
