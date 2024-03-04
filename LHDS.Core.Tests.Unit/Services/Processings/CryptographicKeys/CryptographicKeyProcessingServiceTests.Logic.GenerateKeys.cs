// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CryptographicKeys
{
    internal partial class CryptographicKeyProcessingServiceTests
    {
        [Fact]
        public async Task ShouldGenerateKeysAsync()
        {
            //given
            string randomPublicKeyComment = GetRandomString();
            string inputPublicKeyComment = randomPublicKeyComment;
            string gpgCryptographyType = "GPG";
            string ftpCryptographyType = "FTP";
            CryptographicKey randomGpgCryptographicKey = CreateRandomCryptographicKey();
            CryptographicKey generatedGpgCryptographicKey = randomGpgCryptographicKey;
            CryptographicKey randomFtpCryptographicKey = CreateRandomCryptographicKey();
            CryptographicKey generatedFtpCryptographicKey = randomFtpCryptographicKey;
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential generatedSubscriberCredential = inputSubscriberCredential;
            generatedSubscriberCredential.GpgPassPhrase = generatedGpgCryptographicKey.Passphrase;
            generatedSubscriberCredential.GpgPublicKey = generatedGpgCryptographicKey.Base64PublicKey;
            generatedSubscriberCredential.GpgPrivateKey = generatedGpgCryptographicKey.Base64PrivateKey;
            generatedSubscriberCredential.FtpPassPhrase = generatedGpgCryptographicKey.Passphrase;
            generatedSubscriberCredential.FtpPrivateKey = generatedGpgCryptographicKey.Base64PrivateKey;
            generatedSubscriberCredential.FtpPublicKey = generatedGpgCryptographicKey.Base64PublicKey;
            SubscriberCredential expectedSubscriberCredential = generatedSubscriberCredential;

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync("GPG", inputPublicKeyComment))
                    .ReturnsAsync(generatedGpgCryptographicKey);

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync("FTP", inputPublicKeyComment))
                    .ReturnsAsync(generatedFtpCryptographicKey);

            // when
            SubscriberCredential actualSubscriberCredential =
                await this.cryptographyKeyProcessingService.GenerateKeysAsync(inputSubscriberCredential);

            // then
            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync("GPG", inputPublicKeyComment),
                    Times.Once);

            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync("FTP", inputPublicKeyComment),
                    Times.Once);

            Assert.Equal(expectedSubscriberCredential, actualSubscriberCredential);
        }
    }
}
