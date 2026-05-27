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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings.Download
{
    public partial class IngestionTrackingDownloadHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingDownloadHealthCheckServiceException =
                new FailedIngestionTrackingDownloadHealthCheckServiceException(

                    message:
                        "Failed ingestion tracking download health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingDownloadHealthCheckServiceException =
                new IngestionTrackingDownloadHealthCheckServiceException(

                    message:
                        "Ingestion tracking download health check service error occurred, " +
                        "please contact support.",

                    innerException: failedIngestionTrackingDownloadHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            IngestionTrackingDownloadHealthCheckServiceException
                actualIngestionTrackingDownloadHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingDownloadHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingDownloadHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDownloadHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDownloadHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
