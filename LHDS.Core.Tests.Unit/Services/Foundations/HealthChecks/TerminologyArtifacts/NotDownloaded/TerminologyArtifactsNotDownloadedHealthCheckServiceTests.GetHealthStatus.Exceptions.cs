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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.TerminologyArtifacts.NotDownloaded
{
    public partial class TerminologyArtifactsNotDownloadedHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedTerminologyArtifactsNotDownloadedHealthCheckServiceException =
                new FailedTerminologyArtifactsNotDownloadedHealthCheckServiceException(

                    message:
                        "Failed terminology artifacts not downloaded health check service error occurred, "
                            + "please contact support.",

                    innerException: serviceException);

            var expectedTerminologyArtifactsNotDownloadedHealthCheckServiceException =
                new TerminologyArtifactsNotDownloadedHealthCheckServiceException(

                    message:
                        "Terminology artifacts not downloaded health check service error occurred, "
                            + "please contact support.",

                    innerException: failedTerminologyArtifactsNotDownloadedHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.terminologyArtifactsHealthItemService.GetHealthStatusAsync();

            TerminologyArtifactsNotDownloadedHealthCheckServiceException
                actualTerminologyArtifactsNotDownloadedHealthCheckServiceException =
                    await Assert.ThrowsAsync<TerminologyArtifactsNotDownloadedHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualTerminologyArtifactsNotDownloadedHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactsNotDownloadedHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactsNotDownloadedHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
