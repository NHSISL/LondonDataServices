// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
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
            ShouldThrowAggregateDependencyValidationExceptionOnNonDownloadedIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomUndownloadedTerminologyArtifacts();
            List<TerminologyArtifact> undownloadedTerminologyArtifacts = randomTerminologyArtifacts;
            string inputFileName = undownloadedTerminologyArtifacts.ToString();
            string outputFileName = inputFileName;
            string outputArtifactDetail = GetRandomString();
            List<Exception> exceptions = new List<Exception>();

            foreach (TerminologyArtifact terminologyArtifact in undownloadedTerminologyArtifacts)
            {
                this.terminologyArtifactProcessingServiceMock.Setup(service =>
                    service.GetNonDownloadedArtifactAsync())
                        .ReturnsAsync(terminologyArtifact);

                string relativeUrl = terminologyArtifact.FullUrl;

                this.ontologyProcessingServiceMock.Setup(service =>
                    service.RetrieveArtifactDetailsAsync(relativeUrl))
                        .ThrowsAsync(dependencyValidationException);

                var terminologyDetailOrchestrationDependencyValidationException =
                    new TerminologyDetailOrchestrationDependencyValidationException(
                        message: "Terminology detail orchestration dependency validation error occurred, " +
                        "please try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(terminologyDetailOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve artifact detail for {exceptions.Count} artifacts",
                    exceptions);

            var failedTerminologyDetailOrchestrationServiceException =
                new FailedTerminologyDetailOrchestrationServiceException(
                    message: "Failed terminology detail aggregate orchestration service occurred, " +
                    "please contact support.",
                    innerException: aggregateException);

            var expectedTerminologyDetailOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: failedTerminologyDetailOrchestrationServiceException);

            // When
            ValueTask retrieveArtifactDetailsTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationServiceException actualTerminologyDetailOrchestrationServiceException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationServiceException>(async () =>
                    await retrieveArtifactDetailsTask);

            // Then
            actualTerminologyDetailOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedTerminologyDetailOrchestrationServiceException);

            foreach (TerminologyArtifact terminologyArtifact in undownloadedTerminologyArtifacts)
            {
                this.terminologyArtifactProcessingServiceMock.Verify(service =>
                    service.GetNonDownloadedArtifactAsync(),
                        Times.Once);

                string relativeUrl = terminologyArtifact.FullUrl;

                this.ontologyProcessingServiceMock.Verify(service =>
                    service.RetrieveArtifactDetailsAsync(relativeUrl),
                        Times.Once);
            }

            var terminologyDetailOrchestrationDependencyValidationLoggingException =
                new TerminologyDetailOrchestrationDependencyValidationException(
                    message: "Terminology artifact orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    terminologyDetailOrchestrationDependencyValidationLoggingException))),
                        Times.Exactly(undownloadedTerminologyArtifacts.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualTerminologyDetailOrchestrationServiceException))),
                        Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TerminologyDetailOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnNonDownloadedIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomUndownloadedTerminologyArtifacts();
            List<TerminologyArtifact> undownloadedTerminologyArtifacts = randomTerminologyArtifacts;
            string inputFileName = undownloadedTerminologyArtifacts.ToString();
            string outputFileName = inputFileName;
            string outputArtifactDetail = GetRandomString();
            List<Exception> exceptions = new List<Exception>();

            foreach (TerminologyArtifact terminologyArtifact in undownloadedTerminologyArtifacts)
            {
                this.terminologyArtifactProcessingServiceMock.Setup(service =>
                    service.GetNonDownloadedArtifactAsync())
                        .ReturnsAsync(terminologyArtifact);

                string relativeUrl = terminologyArtifact.FullUrl;

                this.ontologyProcessingServiceMock.Setup(service =>
                    service.RetrieveArtifactDetailsAsync(relativeUrl))
                        .ThrowsAsync(dependencyException);

                var terminologyDetailOrchestrationDependencyException =
                    new TerminologyDetailOrchestrationDependencyException(
                        message: "Terminology detail orchestration dependency validation error occurred, " +
                            "please try again.",
                        innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(terminologyDetailOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve artifact detail for {exceptions.Count} artifacts",
                    exceptions);

            var failedTerminologyDetailOrchestrationServiceException =
                new FailedTerminologyDetailOrchestrationServiceException(
                    message: "Failed terminology detail aggregate orchestration service occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedTerminologyDetailOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: failedTerminologyDetailOrchestrationServiceException);

            // When
            ValueTask retrieveArtifactDetailsTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationServiceException actualTerminologyDetailOrchestrationServiceException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationServiceException>(async () =>
                    await retrieveArtifactDetailsTask);

            // Then
            actualTerminologyDetailOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedTerminologyDetailOrchestrationServiceException);

            foreach (TerminologyArtifact terminologyArtifact in undownloadedTerminologyArtifacts)
            {
                this.terminologyArtifactProcessingServiceMock.Verify(service =>
                    service.GetNonDownloadedArtifactAsync(),
                        Times.Once);

                string relativeUrl = terminologyArtifact.FullUrl;

                this.ontologyProcessingServiceMock.Verify(service =>
                    service.RetrieveArtifactDetailsAsync(relativeUrl),
                        Times.Once);
            }

            var terminologyDetailOrchestrationDependencyLoggingException =
                new TerminologyDetailOrchestrationDependencyException(
                    message: "Terminology artifact orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    terminologyDetailOrchestrationDependencyLoggingException))),
                        Times.Exactly(undownloadedTerminologyArtifacts.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualTerminologyDetailOrchestrationServiceException))),
                        Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomUndownloadedTerminologyArtifacts();
            List<TerminologyArtifact> undownloadedTerminologyArtifacts = randomTerminologyArtifacts;
            string inputFileName = undownloadedTerminologyArtifacts.ToString();
            string outputFileName = inputFileName;
            string outputArtifactDetail = GetRandomString();
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();

            var innerFailedTerminologyDetailOrchestrationServiceException =
                new FailedTerminologyDetailOrchestrationServiceException(
                    message: "Failed terminology detail orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerTerminologyDetailOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Terminology detail orchestration service error occurred, contact support.",
                    innerException: innerFailedTerminologyDetailOrchestrationServiceException);

            foreach (TerminologyArtifact terminologyArtifact in undownloadedTerminologyArtifacts)
            {
                this.terminologyArtifactProcessingServiceMock.Setup(service =>
                    service.GetNonDownloadedArtifactAsync())
                        .ReturnsAsync(terminologyArtifact);

                string relativeUrl = terminologyArtifact.FullUrl;

                this.ontologyProcessingServiceMock.Setup(service =>
                    service.RetrieveArtifactDetailsAsync(relativeUrl))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerTerminologyDetailOrchestrationServiceException);
            }

            var aggregateException =
               new AggregateException(
                   $"Unable to retrieve artifact detail for {exceptions.Count} artifacts",
                   exceptions);

            var failedTerminologyDetailOrchestrationServiceException =
                new FailedTerminologyDetailOrchestrationServiceException(
                    message: "Failed terminology detail aggregate orchestration service occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedTerminologyDetailOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Terminology detail orchestration service error occurred, contact support.",
                    innerException: failedTerminologyDetailOrchestrationServiceException);

            // When
            ValueTask retrieveArtifactDetailsTask =
                this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            TerminologyDetailOrchestrationServiceException actualTerminologyDetailOrchestrationServiceException =
                await Assert.ThrowsAsync<TerminologyDetailOrchestrationServiceException>(async () =>
                    await retrieveArtifactDetailsTask);

            // Then
            actualTerminologyDetailOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedTerminologyDetailOrchestrationServiceException);

            foreach (TerminologyArtifact terminologyArtifact in undownloadedTerminologyArtifacts)
            {
                this.terminologyArtifactProcessingServiceMock.Verify(service =>
                    service.GetNonDownloadedArtifactAsync(),
                        Times.Once());  

                string relativeUrl = terminologyArtifact.FullUrl;

                this.ontologyProcessingServiceMock.Setup(service =>
                    service.RetrieveArtifactDetailsAsync(relativeUrl))
                        .ThrowsAsync(serviceException);
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerTerminologyDetailOrchestrationServiceException))),
                        Times.Exactly(undownloadedTerminologyArtifacts.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyDetailOrchestrationServiceException))),
                        Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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
                    message: "Terminology detail orchestration dependency validation error occurred, " +
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
