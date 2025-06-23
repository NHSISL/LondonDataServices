using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.TerminologyPolls.FailedToProcessHealthCheck
{
    public partial class TerminologyPollsFailedToProcessHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedTerminologyPollsFailedToProcessHealthCheckServiceException =
                new FailedTerminologyPollsFailedToProcessHealthCheckServiceException(

                    message:
                        "Failed terminology polls failed to process health check service error occurred, "
                            + "please contact support.",

                    innerException: serviceException);

            var expectedTerminologyPollsFailedToProcessHealthCheckServiceException =
                new TerminologyPollsFailedToProcessHealthCheckServiceException(

                    message:
                        "Terminology polls failed to process health check service error occurred, "
                            + "please contact support.",

                    innerException: failedTerminologyPollsFailedToProcessHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask = 
                this.terminologyPollsFailedToProcessHealthCheckService.GetHealthStatusAsync();

            TerminologyPollsFailedToProcessHealthCheckServiceException
                actualTerminologyPollsFailedToProcessHealthCheckServiceException =
                    await Assert.ThrowsAsync<TerminologyPollsFailedToProcessHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualTerminologyPollsFailedToProcessHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedTerminologyPollsFailedToProcessHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollsFailedToProcessHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
