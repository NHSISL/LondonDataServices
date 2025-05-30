using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTracking.IngestionTrackingDecryptionHealthCheck
{
    public partial class IngestionTrackingDecryptionHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingDecryptionHealthCheckCooridinationServiceException =
                new FailedIngestionTrackingDecryptionHealthCheckCooridinationServiceException(
                    message: "Failed ingestion tracking decryption health check coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingDecryptionHealthCheckCooridinationServiceException =
                new IngestionTrackingDecryptionHealthCheckCooridinationServiceException(
                    message: "Ingestion tracking decryption health check coordination service error occurred, please contact support.",
                    innerException: failedIngestionTrackingDecryptionHealthCheckCooridinationServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask = this.ingestionTrackingDecryptionHealthCheckService.GetHealthStatusAsync();

            IngestionTrackingDecryptionHealthCheckCooridinationServiceException 
                actualIngestionTrackingDecryptionHealthCheckCooridinationServiceException =
                    await Assert.ThrowsAsync<IngestionTrackingDecryptionHealthCheckCooridinationServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualIngestionTrackingDecryptionHealthCheckCooridinationServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDecryptionHealthCheckCooridinationServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDecryptionHealthCheckCooridinationServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
