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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.OptOuts.PipelineAlive
{
    public partial class OptOutPipelineAliveHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedOptOutPipelineAliveHealthCheckServiceException =
                new FailedOptOutPipelineAliveHealthCheckServiceException(

                    message: "Failed opt out pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedOptOutPipelineAliveHealthCheckServiceException =
                new OptOutPipelineAliveHealthCheckServiceException(

                    message: "Opt out pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: failedOptOutPipelineAliveHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.optOutHealthItemService.GetHealthStatusAsync();

            OptOutPipelineAliveHealthCheckServiceException
                actualOptOutPipelineAliveHealthCheckServiceException =
                    await Assert.ThrowsAsync<OptOutPipelineAliveHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualOptOutPipelineAliveHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedOptOutPipelineAliveHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedOptOutPipelineAliveHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
