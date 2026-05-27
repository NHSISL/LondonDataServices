// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings.PipelineAlive
{
    public partial class IngestionTrackingPipelineAliveHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingPipelineAliveHealthCheckServiceException =
                new FailedIngestionTrackingPipelineAliveHealthCheckServiceException(

                    message: "Failed ingestion tracking pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingPipelineAliveHealthCheckServiceException =
                new IngestionTrackingPipelineAliveHealthCheckServiceException(

                    message: "Ingestion tracking pipeline alive health check service error occurred, " +
                        "please contact support.",

                    innerException: failedIngestionTrackingPipelineAliveHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            IngestionTrackingPipelineAliveHealthCheckServiceException
                actualIngestionTrackingPipelineAliveHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingPipelineAliveHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingPipelineAliveHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingPipelineAliveHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is<IngestionTrackingPipelineAliveHealthCheckServiceException>(
                    actualException => actualException.SameExceptionAs(
                        expectedIngestionTrackingPipelineAliveHealthCheckServiceException))),
                            Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
