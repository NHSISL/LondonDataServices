using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses.MatchingProcess
{
    public partial class ResolvedAddressMatchingProcessHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressMatchingProcessHealthCheckServiceException =
                new FailedResolvedAddressMatchingProcessHealthCheckServiceException(

                    message: "Failed resolved address matching process health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedResolvedAddressMatchingProcessHealthCheckServiceException =
                new ResolvedAddressMatchingProcessHealthCheckServiceException(

                    message: "Resolved address matching process health check service error occurred, " +
                        "please contact support.",

                    innerException: failedResolvedAddressMatchingProcessHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.resolvedAddressHealthItemService.GetHealthStatusAsync();

            ResolvedAddressMatchingProcessHealthCheckServiceException
                actualResolvedAddressMatchingProcessHealthCheckServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressMatchingProcessHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualResolvedAddressMatchingProcessHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressMatchingProcessHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressMatchingProcessHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
