// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfSubscriberCredentialIdIsInvalidAsync()
        {
            // given
            Guid invalidSubscriberCredentialId = Guid.Empty;

            var invalidArgumentSubscriberCredentialOrchestrationException =
                new InvalidArgumentSubscriberCredentialOrchestrationException(
                    message: "Invalid argument subscriber credential orchestration error occurred, please contact support.");

            invalidArgumentSubscriberCredentialOrchestrationException.AddData(
                key: "subscriberCredentialId",
                values: "Id is required");

            var expectedSubscriberCredentialOrchestrationValidationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occurred, please try again.",
                    innerException: invalidArgumentSubscriberCredentialOrchestrationException);

            // when
            ValueTask<SubscriberCredential> retrieveSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RetrieveSubscriberCredentialByIdAsync(
                    invalidSubscriberCredentialId);

            SubscriberCredentialValidationOrchestrationException actuaValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationOrchestrationException>(
                    retrieveSubscriberCredentialTask.AsTask);

            // then
            actuaValidationException.Should().BeEquivalentTo(
                expectedSubscriberCredentialOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSubscriberCredentialOrchestrationValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfSubscriberAgreementIsNullAndLogItAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            SubscriberAgreement nullSubscriberAgreement = null;

            var invalidSubscriberAgreementOrchestrationException =
                new InvalidSubscriberAgreementOrchestrationException(
                    message: "Invalid subscriber agreement orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedSubscriberCredentialValidationOrchestrationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occurred, please try again.",
                    innerException: invalidSubscriberAgreementOrchestrationException);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(randomId)).
                    ReturnsAsync(nullSubscriberAgreement);

            // when
            ValueTask<SubscriberCredential> retrieveSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RetrieveSubscriberCredentialByIdAsync(randomId);

            SubscriberCredentialValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationOrchestrationException>(
                    retrieveSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialValidationOrchestrationException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(randomId),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSubscriberCredentialValidationOrchestrationException))),
                        Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
