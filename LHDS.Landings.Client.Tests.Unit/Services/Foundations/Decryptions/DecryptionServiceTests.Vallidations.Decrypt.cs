// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Decryptions
{
    public partial class DecryptionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDecryptIfDecryptionIsNullAndLogItAsync()
        {
            // given
            byte[] nullDecryption = null;

            var nullDecryptionException =
                new NullDecryptionException();

            var expectedDecryptionValidationException =
                new DecryptionValidationException(nullDecryptionException);

            // when
            Task<byte[]> decryptTask =
                this.decryptionService.DecryptAsync(nullDecryption);

            DecryptionValidationException actualDecryptionValidationException =
                await Assert.ThrowsAsync<DecryptionValidationException>(async () =>
                    await decryptTask);

            // then
            actualDecryptionValidationException.Should()
                .BeEquivalentTo(expectedDecryptionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
