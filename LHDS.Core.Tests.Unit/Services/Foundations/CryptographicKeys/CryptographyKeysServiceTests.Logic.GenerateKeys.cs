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
            string randomCryptographyTypeString = GetRandomString();
            string inputCryptographyType = randomCryptographyTypeString;

            string randomPublicKeyCommentString = GetRandomString();
            string inputPublicKeyCommentString = randomPublicKeyCommentString;

            CryptographicKey randomCryptographicKey = GenerateRandomCryptographicKey();
            CryptographicKey outputCryptographicKey = randomCryptographicKey;
            CryptographicKey expectedCryptographicKey = outputCryptographicKey.DeepClone();

            this.cryptographyKeyBroker.Setup(brokers =>
                brokers.GenerateKeys(inputPublicKeyCommentString))
                    .ReturnsAsync(outputCryptographicKey);

            // When
            CryptographicKey actualCryptographicKey = await this.cryptographyKeyService
               .GenerateKeys(cryptographyType: inputCryptographyType, publicKeyComment: inputPublicKeyCommentString);

            // Then
            actualCryptographicKey.Should().BeEquivalentTo(expectedCryptographicKey);

            this.cryptographyKeyBroker.Verify(broker =>
                broker.GenerateKeys(inputPublicKeyCommentString),
                Times.Once);

            this.cryptographyKeyBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
