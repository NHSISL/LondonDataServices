// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
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
            ShouldThrowAggregateDependencyValidationExceptionOnRetrieveArtifactDetailsIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            List<Exception> exceptions = new List<Exception>();
            string inputContainer = blobContainers.Terminology;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifacts = CreateRandomUndownloadedTerminologyArtifact();
            TerminologyArtifact undownloadedTerminologyArtifact = randomTerminologyArtifacts;
            string outputArtifactDetail = GetRandomString();

            this.terminologyArtifactProcessingServiceMock.SetupSequence(service =>
                service.GetNonDownloadedArtifactAsync())
                    .ReturnsAsync(undownloadedTerminologyArtifact)
                    .ReturnsAsync((TerminologyArtifact?)null);

            this.ontologyProcessingServiceMock.Setup(service =>
                service.RetrieveArtifactDetailsAsync(undownloadedTerminologyArtifact.FullUrl))
                    .ThrowsAsync(dependencyValidationException);

            var terminologyDetailOrchestrationDependencyValidationException =
                new TerminologyDetailOrchestrationDependencyValidationException(
                    message: "Terminology detail orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            exceptions.Add(terminologyDetailOrchestrationDependencyValidationException);

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve terminology artifact details for {exceptions.Count} message IDs",
                    exceptions);

            var failedTerminologyDetailOrchestrationServiceException =
                new FailedTerminologyDetailOrchestrationServiceException(
                    message: "Failed terminology detail aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedOptOutOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Terminology detail orchestration service error occurred, please contact support.",
                    innerException: failedTerminologyDetailOrchestrationServiceException);

            // When
            ValueTask retrieveTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationServiceException>(retrieveTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationServiceException);

            this.terminologyArtifactProcessingServiceMock.Verify(service =>
                service.GetNonDownloadedArtifactAsync(),
                    Times.Exactly(2));

            this.ontologyProcessingServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(undownloadedTerminologyArtifact.FullUrl),
                    Times.Once);

            var optOutOrchestrationDependencyValidationLoggingException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    optOutOrchestrationDependencyValidationLoggingException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualException))),
                        Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TerminologyDetailOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnRetrieveArtifactDetailsIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedDependencyException =
                new TerminologyDetailOrchestrationDependencyValidationException(
                    message:"Terminology detail orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
              service.GetNonDownloadedArtifactAsync())
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask retrieveTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationDependencyValidationException>(
                    retrieveTask.AsTask);

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
                    message: "Terminology detail orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
              service.GetNonDownloadedArtifactAsync())
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask retrieveTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationDependencyException>(retrieveTask.AsTask);

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
                    message: "Failed terminology detail orchestration service error occurred, please contact support.",
                    serviceException);

            var expectedTerminologyDetailOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Terminology detail orchestration service error occurred, please contact support.",
                    failedTerminologyDetailOrchestrationServiceException);

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
              service.GetNonDownloadedArtifactAsync())
                  .ThrowsAsync(serviceException);

            // when
            ValueTask retrieveTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationServiceException>(retrieveTask.AsTask);

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
