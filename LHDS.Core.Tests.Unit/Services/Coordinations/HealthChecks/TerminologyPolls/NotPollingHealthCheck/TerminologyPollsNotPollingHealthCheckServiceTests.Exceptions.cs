using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.TerminologyPolls.NotPollingHealthCheck
{
    public partial class TerminologyPollsNotPollingHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedTerminologyPollsNotPollingHealthCheckServiceException =
                new FailedTerminologyPollsNotPollingHealthCheckServiceException(

                    message:
                        "Failed terminology polls not polling health check coordination service error occurred, "
                        + "please contact support.",

                    innerException: serviceException);

            var expectedTerminologyPollsNotPollingHealthCheckServiceException =
                new TerminologyPollsNotPollingHealthCheckServiceException(

                    message:
                        "Terminology polls not polling health check coordination service error occurred, "
                        + "please contact support.",

                    innerException: failedTerminologyPollsNotPollingHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask = 
                this.terminologyPollsNotPollingHealthCheckService.GetHealthStatusAsync();

            TerminologyPollsNotPollingHealthCheckServiceException
                actualTerminologyPollsNotPollingHealthCheckServiceException =
                    await Assert.ThrowsAsync<TerminologyPollsNotPollingHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualTerminologyPollsNotPollingHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedTerminologyPollsNotPollingHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollsNotPollingHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
