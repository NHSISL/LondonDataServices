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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.OptOuts.StuckOptOuts
{
    public partial class OptOutsStuckHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedOptOutsStuckHealthCheckServiceException =
                new FailedOptOutsStuckHealthCheckServiceException(

                    message: "Failed opt outs stuck health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedOptOutsStuckHealthCheckServiceException =
                new OptOutsStuckHealthCheckServiceException(

                    message: "Opt outs stuck health check service error occurred, " +
                        "please contact support.",

                    innerException: failedOptOutsStuckHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.optOutsHealthItemService.GetHealthStatusAsync();

            OptOutsStuckHealthCheckServiceException actualOptOutsStuckHealthCheckServiceException =
                await Assert.ThrowsAsync<OptOutsStuckHealthCheckServiceException>(
                    getHealthStatusTask.AsTask);

            // then
            actualOptOutsStuckHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedOptOutsStuckHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedOptOutsStuckHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
