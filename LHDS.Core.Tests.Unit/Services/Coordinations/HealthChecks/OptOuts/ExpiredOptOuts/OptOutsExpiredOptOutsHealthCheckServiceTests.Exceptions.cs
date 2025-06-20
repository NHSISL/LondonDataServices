using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.OptOuts.ExpiredOptOuts
{
    public partial class OptOutsExpiredOptOutsHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedOptOutsExpiredOptOutsHealthCheckServiceException =
                new FailedOptOutsExpiredOptOutsHealthCheckServiceException(

                    message:
                        "Failed opt outs expired opt outs health check service error occurred, "
                        + "please contact support.",

                    innerException: serviceException);

            var expectedOptOutsExpiredOptOutsHealthCheckServiceException =
                new OptOutsExpiredOptOutsHealthCheckServiceException(

                    message:
                        "Opt outs expired opt outs health check service error occurred, "
                        + "please contact support.",

                    innerException: failedOptOutsExpiredOptOutsHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask = this.optOutExpiredOptOutHealthCheckService.GetHealthStatusAsync();

            OptOutsExpiredOptOutsHealthCheckServiceException
                actualOptOutsExpiredOptOutsHealthCheckServiceException =
                    await Assert.ThrowsAsync<OptOutsExpiredOptOutsHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualOptOutsExpiredOptOutsHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedOptOutsExpiredOptOutsHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedOptOutsExpiredOptOutsHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
