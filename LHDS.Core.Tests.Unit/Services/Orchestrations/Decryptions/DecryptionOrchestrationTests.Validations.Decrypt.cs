// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnDecryptIfFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            var invalidArgumentDecryptionOrchestrationException =
                new InvalidArgumentDecryptionOrchestrationException();

            invalidArgumentDecryptionOrchestrationException.AddData(
               key: "FileName",
               values: "Text is required");

            var expectedDecryptionOrchestrationFileNameValidationException =
                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentDecryptionOrchestrationException);

            // when
            ValueTask<string> decryptTask =
                this.decryptionOrchestrationService.DecryptAsync(invalidText);

            DecryptionOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationValidationException>(decryptTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDecryptionOrchestrationFileNameValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionOrchestrationFileNameValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
