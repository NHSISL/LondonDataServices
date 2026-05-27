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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings.DeletedWithoutCompletion
{
    public partial class IngestionTrackingDeletedWithoutCompletionHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException =
                new FailedIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException(

                    message:
                        "Failed ingestion tracking deleted without completion health check service " +
                        "error occurred, please contact support.",

                    innerException: serviceException);

            var expectedIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException =
                new IngestionTrackingDeletedWithoutCompletionHealthCheckServiceException(

                    message:
                        "Ingestion tracking deleted without completion health check service error occurred, " +
                        "please contact support.",

                    innerException: failedIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            IngestionTrackingDeletedWithoutCompletionHealthCheckServiceException
                actualIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingDeletedWithoutCompletionHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
