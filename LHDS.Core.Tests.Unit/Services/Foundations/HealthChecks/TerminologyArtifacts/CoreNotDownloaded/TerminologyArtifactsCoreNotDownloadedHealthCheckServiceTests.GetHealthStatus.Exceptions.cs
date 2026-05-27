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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.TerminologyArtifacts.CoreNotDownloaded
{
    public partial class TerminologyArtifactsCoreNotDownloadedHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException =
                new FailedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException(

                    message: "Failed terminology artifacts core not downloaded health check service error " +
                        "occurred, please contact support.",

                    innerException: serviceException);

            var expectedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException =
                new TerminologyArtifactsCoreNotDownloadedHealthCheckServiceException(

                    message: "Terminology artifacts core not downloaded health check service error occurred, " +
                        "please contact support.",

                    innerException: failedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.terminologyArtifactsHealthItemService.GetHealthStatusAsync();

            TerminologyArtifactsCoreNotDownloadedHealthCheckServiceException
                actualTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException =
                    await Assert.ThrowsAsync<TerminologyArtifactsCoreNotDownloadedHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactsCoreNotDownloadedHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
