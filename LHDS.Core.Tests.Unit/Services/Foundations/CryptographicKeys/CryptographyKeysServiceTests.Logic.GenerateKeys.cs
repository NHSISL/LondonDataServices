// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyServiceTests
    {
        [Fact]
        public async Task ShouldGenerateKeysAsync()
        {
            // given
            string inputCryptographyType = GetRandomString();
            string randomPublicKeyCommentString = GetRandomString();
            string randomUserNameString = "";
            string randomPassPhraseString = "";
            string randomEmailString = "";
            string inputPublicKeyCommentString = randomPublicKeyCommentString;
            CryptographicKey randomCryptographicKey = GenerateRandomCryptographicKey();
            CryptographicKey outputCryptographicKey = randomCryptographicKey;
            CryptographicKey expectedCryptographicKey = outputCryptographicKey.DeepClone();

            this.cryptographyKeyBrokerMock.Setup(broker =>
                broker.CryptographyType)
                    .Returns(inputCryptographyType);

            this.cryptographyKeyBrokerMock.Setup(broker =>
                broker.GenerateKeysAsync(
                    inputPublicKeyCommentString,
                    randomPassPhraseString,
                    randomUserNameString,
                    randomEmailString))
                        .ReturnsAsync(outputCryptographicKey);

            // When
            CryptographicKey actualCryptographicKey = await this.cryptographyKeyService
                .GenerateKeysAsync(cryptographyType: inputCryptographyType, comment: inputPublicKeyCommentString);

            // Then
            actualCryptographicKey.Should().BeEquivalentTo(expectedCryptographicKey);

            this.cryptographyKeyBrokerMock.Verify(broker =>
                broker.CryptographyType,
                    Times.Once);

            this.cryptographyKeyBrokerMock.Verify(broker =>
                broker.GenerateKeysAsync(
                    inputPublicKeyCommentString,
                    randomPassPhraseString,
                    randomUserNameString,
                    randomEmailString),
                        Times.Once);

            this.cryptographyKeyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
