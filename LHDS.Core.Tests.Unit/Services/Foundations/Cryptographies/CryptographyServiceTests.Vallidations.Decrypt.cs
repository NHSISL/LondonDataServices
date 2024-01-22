// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Cryptographies
{
    public partial class CryptographyServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDecryptIfDecryptionIsNullAndLogItAsync()
        {
            // given
            byte[] nullDecryption = null;

            var nullDecryptionException =
                new NullCryptographyException(message: "Data is null.");

            var expectedDecryptionValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: nullDecryptionException);

            // when
            Task<byte[]> decryptTask =
                this.cryptographyService.DecryptAsync(nullDecryption);

            CryptographyValidationException actualDecryptionValidationException =
                await Assert.ThrowsAsync<CryptographyValidationException>(async () =>
                    await decryptTask);

            // then
            actualDecryptionValidationException.Should()
                .BeEquivalentTo(expectedDecryptionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.cryptographyBroker.VerifyNoOtherCalls();
        }
    }
}
