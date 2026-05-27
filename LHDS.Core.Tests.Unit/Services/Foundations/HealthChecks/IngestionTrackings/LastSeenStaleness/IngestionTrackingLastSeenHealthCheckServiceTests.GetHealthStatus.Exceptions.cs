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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings.LastSeenStaleness
{
    public partial class IngestionTrackingLastSeenHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingLastSeenHealthCheckServiceException =
                new FailedIngestionTrackingLastSeenHealthCheckServiceException(

                    message: "Failed ingestion tracking last seen health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingLastSeenHealthCheckServiceException =
                new IngestionTrackingLastSeenHealthCheckServiceException(

                    message: "Ingestion tracking last seen health check service error occurred, " +
                        "please contact support.",

                    innerException: failedIngestionTrackingLastSeenHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            IngestionTrackingLastSeenHealthCheckServiceException
                actualIngestionTrackingLastSeenHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingLastSeenHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingLastSeenHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingLastSeenHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingLastSeenHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
