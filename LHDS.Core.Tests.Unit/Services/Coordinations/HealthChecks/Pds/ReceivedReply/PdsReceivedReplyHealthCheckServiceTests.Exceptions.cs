using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.Pds.ReceivedReply
{
    public partial class PdsReceivedReplyHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedPdsReceivedReplyHealthCheckServiceException =
                new FailedPdsReceivedReplyHealthCheckServiceException(

                    message: "Failedpds received reply health check service error occurred, " +
                    "please contact support.",

                    innerException: serviceException);

            var expectedPdsReceivedReplyHealthCheckServiceException =
                new PdsReceivedReplyHealthCheckServiceException(

                    message: "Pds received reply health check service error occurred, " +
                    "please contact support.",

                    innerException: failedPdsReceivedReplyHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.pdsReceivedReplyHealthCheckService.GetHealthStatusAsync();

            PdsReceivedReplyHealthCheckServiceException
                actualPdsReceivedReplyHealthCheckServiceException =
                    await Assert.ThrowsAsync<PdsReceivedReplyHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualPdsReceivedReplyHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedPdsReceivedReplyHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedPdsReceivedReplyHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
