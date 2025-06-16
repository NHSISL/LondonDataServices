// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveFileIfFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            Stream invalidStream = null;
            string invalidFileName = invalidText;
            SubscriberCredential invalidSubscriberCredential = null;

            var invalidArgumentEmisLandingOrchestrationException =
                new InvalidArgumentEmisLandingOrchestrationException(
                    message: "Invalid EMIS landing orchestration argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentEmisLandingOrchestrationException.AddData(
               key: "Output",
               values: "Stream is required");

            invalidArgumentEmisLandingOrchestrationException.AddData(
               key: "FileName",
               values: "Text is required");

            invalidArgumentEmisLandingOrchestrationException.AddData(
               key: "SubscriberCredential",
               values: "SubscriberCredential is required");

            var expectedDownloadOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentEmisLandingOrchestrationException);

            // when
            ValueTask processTask =
                this.emisLandingOrchestrationService
                    .RetrieveDownloadByFileNameAsync(
                        output: invalidStream,
                        fileName: invalidFileName,
                        subscriberCredential: invalidSubscriberCredential);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedDownloadOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
