// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions;
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
            ShouldThrowAggregateDependencyValidationOnRetrieveArtifactMetadatasIfExceptionOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            string[] resourceTypes = new string[] { "CodeSystem", "ValueSet", "ConceptMap" };
            List<Exception> exceptions = new List<Exception>();

            this.terminologyPollProcessingServiceMock.Setup(service =>
              service.RetrieveOrAddTerminologyPollAsync(It.IsAny<string>()))
                  .ThrowsAsync(dependancyValidationException);

            var terminologyMetadataOrchestrationDependencyValidationException =
                new TerminologyMetadataOrchestrationDependencyValidationException(
                    message:
                        "Terminology metadata orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            foreach (string resourceType in resourceTypes)
            {
                exceptions.Add(terminologyMetadataOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve metadata for {exceptions.Count} artifacts.",
                    exceptions);

            var failedTerminologyMetadataOrchestrationServiceException =
                new FailedTerminologyMetadataOrchestrationServiceException(
                    message: "Failed terminology metadata orchestration aggregate service error occurred" +
                        ", please contact support.",
                    innerException: aggregateException);

            var expectedTerminologyMetadataOrchestrationServiceException =
                new TerminologyMetadataOrchestrationServiceException(
                    message:
                        "Terminology metadata orchestration service error occurred, please contact support.",
                    failedTerminologyMetadataOrchestrationServiceException);

            // when
            ValueTask retrieveTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceTypes);

            TerminologyMetadataOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TerminologyMetadataOrchestrationServiceException>(
                    retrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyMetadataOrchestrationServiceException);

            this.terminologyPollProcessingServiceMock.Verify(service =>
              service.RetrieveOrAddTerminologyPollAsync(It.IsAny<string>()),
                Times.Exactly(resourceTypes.Length));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    terminologyMetadataOrchestrationDependencyValidationException))),
                        Times.Exactly(resourceTypes.Length));

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTerminologyMetadataOrchestrationServiceException))),
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
            ShouldThrowAggregateDependencyExceptionOnRetrieveArtifactMetadatasIfExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            string[] resourceTypes = new string[] { "CodeSystem", "ValueSet", "ConceptMap" };
            List<Exception> exceptions = new List<Exception>();

            this.terminologyPollProcessingServiceMock.Setup(service =>
              service.RetrieveOrAddTerminologyPollAsync(It.IsAny<string>()))
                  .ThrowsAsync(dependancyException);

            var terminologyMetadataOrchestrationDependencyException =
                new TerminologyMetadataOrchestrationDependencyException(
                    message: "Terminology metadata orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            foreach (string resourceType in resourceTypes)
            {
                exceptions.Add(terminologyMetadataOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve metadata for {exceptions.Count} artifacts.",
                    exceptions);

            var failedTerminologyMetadataOrchestrationServiceException =
                new FailedTerminologyMetadataOrchestrationServiceException(
                    message: "Failed terminology metadata orchestration aggregate service error occurred" +
                        ", please contact support.",
                    innerException: aggregateException);

            var expectedTerminologyMetadataOrchestrationServiceException =
                new TerminologyMetadataOrchestrationServiceException(
                    message:
                        "Terminology metadata orchestration service error occurred, please contact support.",
                    failedTerminologyMetadataOrchestrationServiceException);

            // when
            ValueTask retrieveTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceTypes);

            TerminologyMetadataOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TerminologyMetadataOrchestrationServiceException>(retrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyMetadataOrchestrationServiceException);

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddTerminologyPollAsync(It.IsAny<string>()),
                    Times.Exactly(resourceTypes.Length));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    terminologyMetadataOrchestrationDependencyException))),
                        Times.Exactly(resourceTypes.Length));

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTerminologyMetadataOrchestrationServiceException))),
                       Times.Once);

            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnRetrieveArtifactMetadatasIfExceptionOccursAndLogItAsync()
        {
            //Given
            string[] resourceTypes = new string[] { "CodeSystem", "ValueSet", "ConceptMap" };
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddTerminologyPollAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            var innerFailedTerminologyMetadataOrchestrationServiceException =
                new FailedTerminologyMetadataOrchestrationServiceException(
                    message: "Failed terminology metadata orchestration service error occurred, please contact support.",
                    serviceException);

            var innerTerminologyMetadataOrchestrationServiceException =
                new TerminologyMetadataOrchestrationServiceException(
                    message: "Terminology metadata orchestration service error occurred, please contact support.",
                    innerFailedTerminologyMetadataOrchestrationServiceException);

            foreach (string resourceType in resourceTypes)
            {
                exceptions.Add(innerTerminologyMetadataOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve metadata for {exceptions.Count} artifacts.",
                    exceptions);

            var failedTerminologyMetadataOrchestrationServiceException =
                new FailedTerminologyMetadataOrchestrationServiceException(
                    message: "Failed terminology metadata orchestration aggregate service error occurred" +
                        ", please contact support.",
                    innerException: aggregateException);

            var expectedTerminologyMetadataOrchestrationServiceException =
                new TerminologyMetadataOrchestrationServiceException(
                    message:
                        "Terminology metadata orchestration service error occurred, please contact support.",
                    failedTerminologyMetadataOrchestrationServiceException);

            // when
            ValueTask retrieveTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceTypes);

            TerminologyMetadataOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TerminologyMetadataOrchestrationServiceException>(retrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyMetadataOrchestrationServiceException);

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddTerminologyPollAsync(It.IsAny<string>()),
                    Times.Exactly(resourceTypes.Length));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerTerminologyMetadataOrchestrationServiceException))),
                        Times.Exactly(resourceTypes.Length));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyMetadataOrchestrationServiceException))),
                        Times.Once);

            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
