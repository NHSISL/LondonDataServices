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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses.PipelineAlive
{
    public partial class ResolvedAddressPipelineAliveHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressPipelineAliveHealthCheckServiceException =
                new FailedResolvedAddressPipelineAliveHealthCheckServiceException(

                    message: "Failed resolved address pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedResolvedAddressPipelineAliveHealthCheckServiceException =
                new ResolvedAddressPipelineAliveHealthCheckServiceException(

                    message: "Resolved address pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: failedResolvedAddressPipelineAliveHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.resolvedAddressHealthItemService.GetHealthStatusAsync();

            ResolvedAddressPipelineAliveHealthCheckServiceException
                actualResolvedAddressPipelineAliveHealthCheckServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressPipelineAliveHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualResolvedAddressPipelineAliveHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressPipelineAliveHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressPipelineAliveHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
