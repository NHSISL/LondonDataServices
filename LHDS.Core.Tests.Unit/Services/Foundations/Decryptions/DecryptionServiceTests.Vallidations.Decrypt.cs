// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Decryptions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Decryptions
{
    public partial class DecryptionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDecryptIfDecryptionIsNullAndLogItAsync()
        {
            // given
            byte[] nullDecryption = null;

            var nullDecryptionException =
                new NullDecryptionException(message: "Decryption is null.");

            var expectedDecryptionValidationException =
                new DecryptionValidationException(
                    message: "Decryption validation errors occurred, please try again.",
                    innerException: nullDecryptionException);

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
