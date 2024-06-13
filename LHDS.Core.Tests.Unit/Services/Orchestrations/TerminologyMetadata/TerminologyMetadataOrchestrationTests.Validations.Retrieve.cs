// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfResourceTypeIsInvalidAndLogItAsync(
            string[] invalidArray)
        {
            // given
            string[] invalidResourceTypes = invalidArray;

            var invalidArgumentTerminologyMetaDataProcessingException =
                new InvalidArgumentTerminologyMetaDataOrchestrationException(
                    message: "Invalid argument terminology metadata orchestration. " +
                    "Please correct the errors and try again.");

            invalidArgumentTerminologyMetaDataProcessingException.AddData(
                key: "resourceTypes",
                values: "Text is required");

            var expectedTerminologyMetadataOrchestrationValidationException =
            new TerminologyMetadataOrchestrationValidationException(
                message: "Terminology metadata orchestration validation errors occurred, please try again.",
                innerException: invalidArgumentTerminologyMetaDataProcessingException);

            // when
            ValueTask retrieveTerminologyMetadataTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(invalidResourceTypes);

            TerminologyMetadataOrchestrationValidationException
                actualTerminologyMetadataOrchestrationValidationException =
                    await Assert.ThrowsAsync<TerminologyMetadataOrchestrationValidationException>(
                        retrieveTerminologyMetadataTask.AsTask);

            //then
            actualTerminologyMetadataOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedTerminologyMetadataOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyMetadataOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateValidationExceptionOnRetrieveIfResourceTypeIsInvalidAndLogItAsync()
        {
            // given
            string[] invalidResourceTypes = [null, "", " "];
            List<Exception> exceptions = new List<Exception>();

            var invalidArgumentTerminologyMetaDataProcessingException =
                new InvalidArgumentTerminologyMetaDataOrchestrationException(
                    message: "Invalid argument terminology metadata orchestration. " +
                    "Please correct the errors and try again.");

            invalidArgumentTerminologyMetaDataProcessingException.AddData(
                key: "resourceType",
                values: "Text is required");

            var terminologyMetadataOrchestrationValidationException =
                new TerminologyMetadataOrchestrationValidationException(
                    message: "Terminology metadata orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentTerminologyMetaDataProcessingException);

            foreach (string resourceType in invalidResourceTypes)
            {
                exceptions.Add(terminologyMetadataOrchestrationValidationException);
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
            ValueTask retrieveTerminologyMetadataTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(invalidResourceTypes);

            TerminologyMetadataOrchestrationServiceException
                actualTerminologyMetadataOrchestrationServiceException =
                    await Assert.ThrowsAsync<TerminologyMetadataOrchestrationServiceException>(
                        retrieveTerminologyMetadataTask.AsTask);

            //then
            actualTerminologyMetadataOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedTerminologyMetadataOrchestrationServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    terminologyMetadataOrchestrationValidationException))),
                        Times.Exactly(invalidResourceTypes.Length));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyMetadataOrchestrationServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowAggregateValidationExceptionOnRetrieveIfResourceURLIsInvalidAndLogItAsync(
            string? invalidString)
        {
            // given
            string resourceType = "CodeSystem";
            string[] resourceTypes = [resourceType];
            List<Exception> exceptions = new List<Exception>();
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            string? invalidResourceURL = invalidString;
            TerminologyPoll termionologyPoll = CreateRandomTerminologyPoll(resourceType, dateTimeOffset);
            this.ontologyConfiguration.TerminologyServerResourceRelativeUrl = invalidResourceURL;

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType))
                    .ReturnsAsync(termionologyPoll);

            var invalidArgumentTerminologyMetaDataProcessingException =
                new InvalidArgumentTerminologyMetaDataOrchestrationException(
                    message: "Invalid argument terminology metadata orchestration. " +
                    "Please correct the errors and try again.");

            invalidArgumentTerminologyMetaDataProcessingException.AddData(
                key: "resourceURL",
                values: "Text is required");

            var terminologyMetadataOrchestrationValidationException =
                new TerminologyMetadataOrchestrationValidationException(
                    message: "Terminology metadata orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentTerminologyMetaDataProcessingException);

            foreach (string item in resourceTypes)
            {
                exceptions.Add(terminologyMetadataOrchestrationValidationException);
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
            ValueTask retrieveTerminologyMetadataTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceTypes);

            TerminologyMetadataOrchestrationServiceException
                actualTerminologyMetadataOrchestrationServiceException =
                    await Assert.ThrowsAsync<TerminologyMetadataOrchestrationServiceException>(
                        retrieveTerminologyMetadataTask.AsTask);

            //then
            actualTerminologyMetadataOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedTerminologyMetadataOrchestrationServiceException);

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType),
                    Times.Once());


            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    terminologyMetadataOrchestrationValidationException))),
                        Times.Exactly(resourceTypes.Length));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyMetadataOrchestrationServiceException))),
                        Times.Once);

            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}