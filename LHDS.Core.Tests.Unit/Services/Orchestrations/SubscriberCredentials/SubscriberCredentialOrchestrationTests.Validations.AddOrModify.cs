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
        public async Task ShouldThrowValidationExceptionOnModifyOrAddIfSubscriberCredentialIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential nullSubscriberCredential = null;

            var invalidSubscriberCredentialOrchestrationException =
                new InvalidSubscriberCredentialOrchestrationException(
                    message: "Null subscriber credential orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedSubscriberCredentialValidationOrchestrationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occurred, please try again.",
                    innerException: invalidSubscriberCredentialOrchestrationException);

            // when
            ValueTask<SubscriberCredential> addOrModifySubscriberCredentialTask =
                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(nullSubscriberCredential);

            SubscriberCredentialValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationOrchestrationException>(
                    addOrModifySubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberCredentialValidationOrchestrationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyOrAddIfSubscriberAgreementIsNullAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);
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
                service.ModifyOrAddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>())).
                    ReturnsAsync(nullSubscriberAgreement);

            // when
            ValueTask<SubscriberCredential> addOrModifySubscriberCredentialTask =
                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredentialValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationOrchestrationException>(
                    addOrModifySubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialValidationOrchestrationException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.ModifyOrAddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberCredentialValidationOrchestrationException))),
                        Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
