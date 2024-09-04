// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnDecryptFileIfFileNameIsNullAndLogItAsync(string? invalidData)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;

            var invalidArgumentDecryptionCoordinationException =
                new InvalidArgumentDecryptionCoordinationException(
                    message: "Invalid decryption coordination argument, please correct the errors and try again.");

            var expectedDecryptionCoordinationValidationException =
                new DecryptionCoordinationValidationException(
                    message: "Decryption coordination validation error occurred, please try again.",
                    innerException: invalidArgumentDecryptionCoordinationException);

            // when
            ValueTask<string> processDataTask =
                this.decryptionCoordinationService.DecryptAsync(invalidData);

            DecryptionCoordinationValidationException actualDecryptionCoordinationValidationException =
                await Assert.ThrowsAsync<DecryptionCoordinationValidationException>(async () =>
                    await processDataTask);

            // then
            actualDecryptionCoordinationValidationException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationValidationException);

            this.decryptionOrchestrationServiceMock.Verify(service =>
                service.DecryptAsync(invalidData, inputSubscriberCredential),
                    Times.Never());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionCoordinationValidationException))),
                        Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}