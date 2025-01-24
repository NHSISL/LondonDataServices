// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveListIfSubscriptionCredentialIsNullAndLogItAsync()
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
            ValueTask<List<string>> retrieveListOfDocumentsToProcessTask =
                this.emisLandingOrchestrationService
                    .RetrieveListOfDocumentsToProcessAsync(subscriberCredential: inputSubscriberCredential);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(
                    retrieveListOfDocumentsToProcessTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
