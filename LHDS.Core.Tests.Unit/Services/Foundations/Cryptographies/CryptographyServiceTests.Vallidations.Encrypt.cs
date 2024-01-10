// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnEncryptIfEncryptionIsNullAndLogItAsync()
        {
            // given
            byte[] nullEncryption = null;

            var nullEncryptionException =
                new NullCryptographyException(message: "Data is null.");

            var expectedEncryptionValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: nullEncryptionException);

            // when
            Task<byte[]> decryptTask =
                this.cryptographyService.EncryptAsync(nullEncryption);

            CryptographyValidationException actualEncryptionValidationException =
                await Assert.ThrowsAsync<CryptographyValidationException>(async () =>
                    await decryptTask);

            // then
            actualEncryptionValidationException.Should()
                .BeEquivalentTo(expectedEncryptionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEncryptionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.cryptographyBroker.VerifyNoOtherCalls();
        }
    }
}
