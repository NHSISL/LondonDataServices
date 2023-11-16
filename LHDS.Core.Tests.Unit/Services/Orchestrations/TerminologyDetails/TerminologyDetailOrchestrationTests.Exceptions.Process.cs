//---------------------------------------------------------------
//Copyright(c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyDetails
{
    public partial class TerminologyDetailOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(TerminologyDetailOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnRetrieveArtifactDetailsIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedDependencyException =
                new TerminologyDetailOrchestrationDependencyValidationException(
                    message:
                        "Terminology detail orchestration dependency validation error occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
              service.GetNonDownloadedArtifactAsync())
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask retrireveTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationDependencyValidationException>(retrireveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.terminologyArtifactProcessingServiceMock.Verify(service =>
              service.GetNonDownloadedArtifactAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TerminologyDetailOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveArtifactDetailsIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new TerminologyDetailOrchestrationDependencyException(
                    message: "Terminology detail orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
              service.GetNonDownloadedArtifactAsync())
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask retrireveTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationDependencyException>(retrireveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.terminologyArtifactProcessingServiceMock.Verify(service =>
              service.GetNonDownloadedArtifactAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnRetrieveArtifactDetailsIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            var serviceException = new Exception();

            var failedTerminologyDetailOrchestrationServiceException =
                new FailedTerminologyDetailOrchestrationServiceException(
                    message: "Failed terminology detail orchestration service occurred, please contact support",
                    serviceException);

            var expectedTerminologyDetailOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Terminology detail orchestration service error occurred, contact support.",
                    failedTerminologyDetailOrchestrationServiceException);

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
              service.GetNonDownloadedArtifactAsync())
                  .ThrowsAsync(serviceException);

            // when
            ValueTask retrireveTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationServiceException>(retrireveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyDetailOrchestrationServiceException);

            this.terminologyArtifactProcessingServiceMock.Verify(service =>
              service.GetNonDownloadedArtifactAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyDetailOrchestrationServiceException))),
                        Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
