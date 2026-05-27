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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses.Queued
{
    public partial class ResolvedAddressQueuedHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressQueuedHealthCheckServiceException =
                new FailedResolvedAddressQueuedHealthCheckServiceException(

                    message:
                        "Failed resolved address queued health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedResolvedAddressQueuedHealthCheckServiceException =
                new ResolvedAddressQueuedHealthCheckServiceException(

                    message:
                        "Resolved address queued health check service error occurred, " +
                        "please contact support.",

                    innerException: failedResolvedAddressQueuedHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.resolvedAddressHealthItemService.GetHealthStatusAsync();

            ResolvedAddressQueuedHealthCheckServiceException
                actualResolvedAddressQueuedHealthCheckServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressQueuedHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualResolvedAddressQueuedHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressQueuedHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressQueuedHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
