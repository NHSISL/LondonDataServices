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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings.StaleLastBatchCheck
{
    public partial class IngestionTrackingStaleLastBatchCheckHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException =
                new FailedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException(

                    message:
                        "Failed ingestion tracking stale last batch check health check service " +
                        "error occurred, please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException =
                new IngestionTrackingStaleLastBatchCheckHealthCheckServiceException(

                    message:
                        "Ingestion tracking stale last batch check health check service error occurred, " +
                        "please contact support.",

                    innerException: failedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            IngestionTrackingStaleLastBatchCheckHealthCheckServiceException
                actualIngestionTrackingStaleLastBatchCheckHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingStaleLastBatchCheckHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingStaleLastBatchCheckHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingStaleLastBatchCheckHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
