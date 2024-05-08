// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionRemoveIfSubscriberCredentialIdIsInvalidAsync()
        {
            // given
            Guid invalidSubscriberCredentialId = Guid.Empty;

            var invalidArgumentSubscriberCredentialProcessingException =
                new InvalidArgumentSubscriberCredentialProcessingException(
                    message: "Invalid argument subscriber credential processing error occurred, please contact support.");

            invalidArgumentSubscriberCredentialProcessingException.AddData(
                key: "subscriberCredentialId",
                values: "Id is required");

            var expectedSubscriberCredentialValidationException =
                new SubscriberCredentialValidationException(
                    message: "Subscriber credential validation errors occurred, please try again.",
                    innerException: invalidArgumentSubscriberCredentialProcessingException);

            // when
            ValueTask removeSubscriberCredentialTask =
                this.secureDataProcessingService.RemoveSecureDataByIdAsync(invalidSubscriberCredentialId);

            SubscriberCredentialValidationException actualSubscriberCredentialValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationException>(
                    removeSubscriberCredentialTask.AsTask);

            // then
            actualSubscriberCredentialValidationException.Should().BeEquivalentTo(
                expectedSubscriberCredentialValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSubscriberCredentialValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataServiceMock.VerifyNoOtherCalls();
        }
    }
}