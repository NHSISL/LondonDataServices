// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveFileIfSubscriptionCredentialIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential inputSubscriberCredential = null;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            var nullSubscriberCredentialEmisLandingOrchestrationException =
                new NullSubscriberCredentialEmisLandingOrchestrationException(
                    message: "Null subscriber credential EMIS landing orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedEmisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: nullSubscriberCredentialEmisLandingOrchestrationException);

            // when
            ValueTask<byte[]> processTask =
                this.emisLandingOrchestrationService
                    .RetrieveDownloadByFileNameAsync(
                        fileName: inputFileName,
                        subscriberCredential: inputSubscriberCredential);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveFileIfFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string inputFileName = invalidText;

            var invalidArgumentEmisLandingOrchestrationException =
                new InvalidArgumentEmisLandingOrchestrationException(
                    message: "Invalid EMIS landing orchestration argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentEmisLandingOrchestrationException.AddData(
               key: "FileName",
               values: "Text is required");

            var expectedDownloadOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentEmisLandingOrchestrationException);

            // when
            ValueTask<byte[]> processTask =
                this.emisLandingOrchestrationService
                    .RetrieveDownloadByFileNameAsync(
                        fileName: inputFileName,
                        subscriberCredential: inputSubscriberCredential);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedDownloadOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
        }
    }
}
