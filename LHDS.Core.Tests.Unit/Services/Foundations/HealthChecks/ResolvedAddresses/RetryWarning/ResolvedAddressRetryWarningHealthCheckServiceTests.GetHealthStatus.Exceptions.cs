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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses.RetryWarning
{
    public partial class ResolvedAddressRetryWarningHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressRetryWarningHealthCheckServiceException =
                new FailedResolvedAddressRetryWarningHealthCheckServiceException(

                    message: "Failed resolved address retry warning health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedResolvedAddressRetryWarningHealthCheckServiceException =
                new ResolvedAddressRetryWarningHealthCheckServiceException(

                    message: "Resolved address retry warning health check service error occurred, " +
                        "please contact support.",

                    innerException: failedResolvedAddressRetryWarningHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.resolvedAddressHealthItemService.GetHealthStatusAsync();

            ResolvedAddressRetryWarningHealthCheckServiceException
                actualResolvedAddressRetryWarningHealthCheckServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressRetryWarningHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualResolvedAddressRetryWarningHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressRetryWarningHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressRetryWarningHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
