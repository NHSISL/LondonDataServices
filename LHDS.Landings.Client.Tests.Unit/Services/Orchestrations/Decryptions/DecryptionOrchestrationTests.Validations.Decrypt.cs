// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Orchestrations.Decryptions.Exceptions;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDecryptIfFileNameIsNullAndLogItAsync()
        {
            // given
            string nullFileName = null;

            var nullFileNameException =
                new NullDecryptionOrchestrationFileNameException();

            var expectedDecryptionOrchestrationFileNameValidationException =
                new DecryptionOrchestrationValidationException(nullFileNameException);

            // when
            ValueTask decryptTask =
                this.decryptionOrchestrationService.DecryptAsync(nullFileName);

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
