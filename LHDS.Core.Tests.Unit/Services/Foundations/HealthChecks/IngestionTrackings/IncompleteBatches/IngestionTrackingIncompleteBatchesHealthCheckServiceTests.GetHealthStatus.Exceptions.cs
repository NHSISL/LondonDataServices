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
    public partial class IngestionTrackingIncompleteBatchesHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingIncompleteBatchHealthCheckServiceException =
                new FailedIngestionTrackingIncompleteBatchHealthCheckServiceException(

                    message: 
                        "Failed ingestion tracking incomplete batch health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingIncompleteBatchHealthCheckServiceException =
                new IngestionTrackingIncompleteBatchHealthCheckServiceException(

                    message: 
                        "Ingestion tracking incomplete batch health check service error occurred, " +
                        "please contact support.",

                    innerException: failedIngestionTrackingIncompleteBatchHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            IngestionTrackingIncompleteBatchHealthCheckServiceException
                actualIngestionTrackingIncompleteBatchHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingIncompleteBatchHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingIncompleteBatchHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingIncompleteBatchHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingIncompleteBatchHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}