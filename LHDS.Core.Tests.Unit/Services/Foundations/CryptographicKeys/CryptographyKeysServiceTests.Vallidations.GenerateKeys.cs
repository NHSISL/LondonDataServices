// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnGenerateIfCryptographyTypeIsNullAndLogItAsync()
        {
            // given
            string nullCryptographyType = null;
            string randomPublicKeyCommentString = GetRandomString();

            var nullCryptographyTypeException =
                new NullCryptographyTypeCryptographyKeyException(message: "Cryptography type is null.");

            var expectedCryptographyTypeValidationException =
                new CryptographyKeyValidationException(
                    message: "Cryptography key validation errors occurred, please try again.",
                    innerException: nullCryptographyTypeException);

            // when
            ValueTask<CryptographicKey> CryptographicKeyTask = this.cryptographyKeyService.GenerateKeys(
                cryptographyType: nullCryptographyType,
                publicKeyComment: randomPublicKeyCommentString);

            CryptographyKeyValidationException actualEncryptionValidationException =
                await Assert.ThrowsAsync<CryptographyKeyValidationException>(async () =>
                    await CryptographicKeyTask);

            // then
            actualEncryptionValidationException.Should()
                .BeEquivalentTo(expectedCryptographyTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCryptographyTypeValidationException))),
                        Times.Once);

            this.cryptographyKeyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnGenerateIfPublicKeyCommentIsNullAndLogItAsync()
        {
            // given
            string nullCryptographyType = GetRandomString(); ;
            string randomPublicKeyCommentString = null;

            var nullCryptographyTypeException =
                new NullPublicKeyCommentCryptographyKeyException(message: "Public key comment is null.");

            var expectedCryptographyTypeValidationException =
                new CryptographyKeyValidationException(
                    message: "Cryptography key validation errors occurred, please try again.",
                    innerException: nullCryptographyTypeException);

            // when
            ValueTask<CryptographicKey> CryptographicKeyTask = this.cryptographyKeyService.GenerateKeys(
                cryptographyType: nullCryptographyType,
                publicKeyComment: randomPublicKeyCommentString);

            CryptographyKeyValidationException actualEncryptionValidationException =
                await Assert.ThrowsAsync<CryptographyKeyValidationException>(async () =>
                    await CryptographicKeyTask);

            // then
            actualEncryptionValidationException.Should()
                .BeEquivalentTo(expectedCryptographyTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCryptographyTypeValidationException))),
                        Times.Once);

            this.cryptographyKeyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
