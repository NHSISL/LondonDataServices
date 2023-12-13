// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.TerminologyMetadata.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(TerminologyMetadataOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnRetrieveArtifactMetadatasIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            string resourceType = GetRandomString();

            var expectedDependencyException =
                new TerminologyMetadataOrchestrationDependencyValidationException(
                    message:
                        "Terminology metadata orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.terminologyPollProcessingServiceMock.Setup(service =>
              service.RetrieveOrAddTerminologyPollAsync(resourceType))
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask retrireveTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceType);

            TerminologyMetadataOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyMetadataOrchestrationDependencyValidationException>(
                    retrireveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.terminologyPollProcessingServiceMock.Verify(service =>
              service.RetrieveOrAddTerminologyPollAsync(resourceType),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TerminologyMetadataOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveArtifactMetadatasIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            string resourceType = GetRandomString();

            var expectedDependencyException =
                new TerminologyMetadataOrchestrationDependencyException(
                    message: "Terminology metadata orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.terminologyPollProcessingServiceMock.Setup(service =>
              service.RetrieveOrAddTerminologyPollAsync(resourceType))
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask retrireveTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceType);

            TerminologyMetadataOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<TerminologyMetadataOrchestrationDependencyException>(retrireveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveArtifactMetadatasIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            string resourceType = GetRandomString();
            var serviceException = new Exception();

            var failedTerminologyMetadataOrchestrationServiceException =
                new FailedTerminologyMetadataOrchestrationServiceException(
                    message: "Failed terminology metadata orchestration service occurred, please contact support",
                    serviceException);

            var expectedTerminologyMetadataOrchestrationServiceException =
                new TerminologyMetadataOrchestrationServiceException(
                    message: "Terminology metadata orchestration service error occurred, contact support.",
                    failedTerminologyMetadataOrchestrationServiceException);

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask retrireveTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceType);

            TerminologyMetadataOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TerminologyMetadataOrchestrationServiceException>(retrireveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyMetadataOrchestrationServiceException);

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyMetadataOrchestrationServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
