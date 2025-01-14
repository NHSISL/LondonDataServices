// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnGenerateIfArgumentsIsInvalidAndLogItAsync(string invalidValue)
        {
            // given
            string nullCryptographyType = invalidValue;
            string randomPublicKeyCommentString = invalidValue;

            var invalidArgumentCryptographyKeyException =
                new InvalidArgumentCryptographyKeyException(
                    message: "Invalid cryptography key argument(s), please correct the errors and try again.");

            invalidArgumentCryptographyKeyException.AddData(
                key: "cryptographyType",
                values: "Text is required");

            var expectedCryptographyTypeValidationException =
                new CryptographyKeyValidationException(
                    message: "Cryptography key validation errors occurred, please try again.",
                    innerException: invalidArgumentCryptographyKeyException);

            // when
            ValueTask<CryptographicKey> CryptographicKeyTask = this.cryptographyKeyService.GenerateKeysAsync(
                cryptographyType: nullCryptographyType,
                comment: randomPublicKeyCommentString);

            CryptographyKeyValidationException actualEncryptionValidationException =
                await Assert.ThrowsAsync<CryptographyKeyValidationException>(async () =>
                    await CryptographicKeyTask);

            // then
            actualEncryptionValidationException.Should()
                .BeEquivalentTo(expectedCryptographyTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedCryptographyTypeValidationException))),
                        Times.Once);

            this.cryptographyKeyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnGenerateIfBrokerIsNullAndLogItAsync()
        {
            // given
            string nullCryptographyType = GetRandomString();
            string randomPublicKeyCommentString = GetRandomString();

            var nullCryptographyBrokerException =
                new NullBrokerCryptographyKeyException(message: "Broker is null.");

            var expectedCryptographyTypeValidationException =
                new CryptographyKeyValidationException(
                    message: "Cryptography key validation errors occurred, please try again.",
                    innerException: nullCryptographyBrokerException);

            // when
            ValueTask<CryptographicKey> CryptographicKeyTask = this.cryptographyKeyService.GenerateKeysAsync(
                cryptographyType: nullCryptographyType,
                comment: randomPublicKeyCommentString);

            CryptographyKeyValidationException actualEncryptionValidationException =
                await Assert.ThrowsAsync<CryptographyKeyValidationException>(async () =>
                    await CryptographicKeyTask);

            // then
            actualEncryptionValidationException.Should()
                .BeEquivalentTo(expectedCryptographyTypeValidationException);

            this.cryptographyKeyBrokerMock.Verify(broker =>
               broker.CryptographyType,
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedCryptographyTypeValidationException))),
                        Times.Once);

            this.cryptographyKeyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
