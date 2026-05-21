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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.PdsAudits.PipelineAlive
{
    public partial class PdsAuditPipelineAliveHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedPdsAuditPipelineAliveHealthCheckServiceException =
                new FailedPdsAuditPipelineAliveHealthCheckServiceException(

                    message: "Failed PDS audit pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedPdsAuditPipelineAliveHealthCheckServiceException =
                new PdsAuditPipelineAliveHealthCheckServiceException(

                    message: "PDS audit pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: failedPdsAuditPipelineAliveHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.pdsHealthItemService.GetHealthStatusAsync();

            PdsAuditPipelineAliveHealthCheckServiceException
                actualPdsAuditPipelineAliveHealthCheckServiceException =
                    await Assert.ThrowsAsync<PdsAuditPipelineAliveHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualPdsAuditPipelineAliveHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedPdsAuditPipelineAliveHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditPipelineAliveHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
