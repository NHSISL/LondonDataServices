// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CryptographicKeys
{
    public partial class CryptographicKeyProcessingServiceTests
    {
        [Fact]
        public async Task ShouldGenerateKeysAsync()
        {
            //given
            string randomPublicKeyComment = GetRandomString();
            string inputPublicKeyComment = "";
            string gpgCryptographyType = "GPG";
            string ftpCryptographyType = "SSH";
            CryptographicKey randomGpgCryptographicKey = CreateRandomCryptographicKey();
            CryptographicKey generatedGpgCryptographicKey = randomGpgCryptographicKey;
            CryptographicKey randomFtpCryptographicKey = CreateRandomCryptographicKey();
            CryptographicKey generatedFtpCryptographicKey = randomFtpCryptographicKey;
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential generatedSubscriberCredential = inputSubscriberCredential.DeepClone();
            generatedSubscriberCredential.GpgPassPhrase = generatedGpgCryptographicKey.Passphrase;
            generatedSubscriberCredential.GpgPublicKey = generatedGpgCryptographicKey.Base64PublicKey;
            generatedSubscriberCredential.GpgPrivateKey = generatedGpgCryptographicKey.Base64PrivateKey;
            generatedSubscriberCredential.FtpPassPhrase = generatedFtpCryptographicKey.Passphrase;
            generatedSubscriberCredential.FtpPrivateKey = generatedFtpCryptographicKey.Base64PrivateKey;
            generatedSubscriberCredential.FtpPublicKey = generatedFtpCryptographicKey.Base64PublicKey;
            SubscriberCredential expectedSubscriberCredential = generatedSubscriberCredential;

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync(gpgCryptographyType, inputPublicKeyComment))
                    .ReturnsAsync(generatedGpgCryptographicKey);

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync(ftpCryptographyType, inputPublicKeyComment))
                    .ReturnsAsync(generatedFtpCryptographicKey);

            // when
            SubscriberCredential actualSubscriberCredential =
                await this.cryptographyKeyProcessingService.GenerateKeysAsync(inputSubscriberCredential);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync(gpgCryptographyType, inputPublicKeyComment),
                    Times.Once);

            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync(ftpCryptographyType, inputPublicKeyComment),
                    Times.Once);

            this.cryptographyKeyServiceMock.VerifyNoOtherCalls();
        }
    }
}
