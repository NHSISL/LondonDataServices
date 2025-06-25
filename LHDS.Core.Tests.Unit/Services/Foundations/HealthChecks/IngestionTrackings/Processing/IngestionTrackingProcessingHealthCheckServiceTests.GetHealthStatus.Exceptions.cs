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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings
{
    public partial class IngestionTrackingProcessingHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingProcessingHealthCheckServiceException =
                new FailedIngestionTrackingProcessingHealthCheckServiceException(

                    message: "Failed ingestion tracking processing health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingProcessingHealthCheckServiceException =
                new IngestionTrackingProcessingHealthCheckServiceException(

                    message: "Ingestion tracking processing health check service error occurred, " +
                        "please contact support.",

                    innerException: failedIngestionTrackingProcessingHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            IngestionTrackingProcessingHealthCheckServiceException
                actualIngestionTrackingProcessingHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingProcessingHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingProcessingHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingProcessingHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingProcessingHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}