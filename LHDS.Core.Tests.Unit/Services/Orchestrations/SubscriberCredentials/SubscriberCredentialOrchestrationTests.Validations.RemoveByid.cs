// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionRemoveByIdIfSubscriberCredentialIdIsInvalidAsync()
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
            ValueTask removeSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RemoveSubscriberCredentialByIdAsync(
                    invalidSubscriberCredentialId);

            SubscriberCredentialValidationOrchestrationException actuaValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationOrchestrationException>(
                    removeSubscriberCredentialTask.AsTask);

            // then
            actuaValidationException.Should().BeEquivalentTo(
                expectedSubscriberCredentialOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedSubscriberCredentialOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
