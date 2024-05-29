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
            string inputPassPhrase = "";
            string inputUsername = "";
            string inputEmail = "";
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
            generatedSubscriberCredential.GpgPublicKey = ConvertToBase64(generatedGpgCryptographicKey.PublicKey);
            generatedSubscriberCredential.GpgPrivateKey = ConvertToBase64(generatedGpgCryptographicKey.PrivateKey);
            generatedSubscriberCredential.FtpPassPhrase = generatedFtpCryptographicKey.Passphrase;
            generatedSubscriberCredential.FtpPrivateKey = ConvertToBase64(generatedFtpCryptographicKey.PrivateKey);
            generatedSubscriberCredential.FtpPublicKey = ConvertToBase64(generatedFtpCryptographicKey.PublicKey);
            SubscriberCredential expectedSubscriberCredential = generatedSubscriberCredential;

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync(
                    gpgCryptographyType,
                    inputPublicKeyComment,
                    inputPassPhrase,
                    inputUsername,
                    inputEmail))
                        .ReturnsAsync(generatedGpgCryptographicKey);

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync(
                    ftpCryptographyType,
                    inputPublicKeyComment,
                    inputPassPhrase,
                    inputUsername,
                    inputEmail))
                        .ReturnsAsync(generatedFtpCryptographicKey);

            // when
            SubscriberCredential actualSubscriberCredential =
                await this.cryptographyKeyProcessingService.GenerateKeysAsync(inputSubscriberCredential);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync(
                    gpgCryptographyType,
                    inputPublicKeyComment,
                    inputPassPhrase,
                    inputUsername,
                    inputEmail),
                        Times.Once);

            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync(
                    ftpCryptographyType,
                    inputPublicKeyComment,
                    inputPassPhrase,
                    inputUsername,
                    inputEmail),
                        Times.Once);

            this.cryptographyKeyServiceMock.VerifyNoOtherCalls();
        }
    }
}
