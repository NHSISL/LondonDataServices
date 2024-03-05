// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
                    message: "Invalid argument subscriber credential orchestration error occurred, contact support.");

            invalidArgumentSubscriberCredentialOrchestrationException.AddData(
                key: "subscriberCredentialId",
                values: "Id is required");

            var expectedSubscriberCredentialOrchestrationValidationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occurred, please try again.",
                    innerException: invalidArgumentSubscriberCredentialOrchestrationException);

            // when
            ValueTask<SubscriberCredential> removeSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RetrieveSubscriberCredentialByIdAsync(
                    invalidSubscriberCredentialId);

            SubscriberCredentialValidationOrchestrationException actuaValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationOrchestrationException>(
                    removeSubscriberCredentialTask.AsTask);

            // then
            actuaValidationException.Should().BeEquivalentTo(
                expectedSubscriberCredentialOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSubscriberCredentialOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
