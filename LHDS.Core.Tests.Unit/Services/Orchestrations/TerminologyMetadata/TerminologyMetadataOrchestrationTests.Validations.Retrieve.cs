// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfResourceTypeIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            string inputString = invalidString;

            var invalidArgumentTerminologyMetaDataProcessingException =
                new InvalidArgumentTerminologyMetaDataOrchestrationException(
                    message: "Invalid argument terminology metadata orchestration. " +
                    "Please correct the errors and try again.");

            invalidArgumentTerminologyMetaDataProcessingException.AddData(
                key: "resourceType",
                values: "Text is required");

            var expectedTerminologyMetadataOrchestrationValidationException =
            new TerminologyMetadataOrchestrationValidationException(
                message: "Terminology metadata orchestration validation errors occurred, please try again.",
                innerException: invalidArgumentTerminologyMetaDataProcessingException);

            // when
            ValueTask retrieveTerminologyMetadataTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(inputString);

            TerminologyMetadataOrchestrationValidationException
                actualTerminologyMetadataOrchestrationValidationException =
                    await Assert.ThrowsAsync<TerminologyMetadataOrchestrationValidationException>(() =>
                        retrieveTerminologyMetadataTask.AsTask());

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfResourceURLIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            string resourceType = GetRandomString();
            string invalidResourceURL = invalidString;
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

            var expectedTerminologyMetadataOrchestrationValidationException =
                new TerminologyMetadataOrchestrationValidationException(
                    message: "Terminology metadata orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentTerminologyMetaDataProcessingException);

            // when
            ValueTask retrieveTerminologyMetadataTask =
                this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceType);

            TerminologyMetadataOrchestrationValidationException
                actualTerminologyMetadataOrchestrationValidationException =
                    await Assert.ThrowsAsync<TerminologyMetadataOrchestrationValidationException>(() =>
                        retrieveTerminologyMetadataTask.AsTask());

            //then
            actualTerminologyMetadataOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedTerminologyMetadataOrchestrationValidationException);

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyMetadataOrchestrationValidationException))),
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