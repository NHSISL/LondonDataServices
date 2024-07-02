// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Xunit;

namespace LHDS.Core.Tests.Unit.Providers.Cryptography.Gpg
{
    public partial class GpgCryptographyProviderTests
    {
        [Fact]
        public async Task ShouldEncryptAndDecryptStreamAsync()
        {
            // Given
            var gpgKeyBroker = new GpgKeyBroker();
            var keys = await gpgKeyBroker.GenerateKeysAsync("Test");

            var subscriberCredential = new SubscriberCredential
            {
                GpgPrivateKey = ConvertToBase64String(keys.PrivateKey),
                GpgPublicKey = ConvertToBase64String(keys.PublicKey),
                GpgPassPhrase = keys.Passphrase,
            };

            string randomString = GetRandomString();
            string expectedString = randomString;
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            Stream unencryptedStream = new MemoryStream(randomBytes);
            Stream encryptedStream = new MemoryStream();
            Stream decryptedStream = new MemoryStream();

            // When
            await this.cryptographyProvider.EncryptAsync(
                input: unencryptedStream,
                output: encryptedStream,
                subscriberCredential);

            await this.cryptographyProvider.DecryptAsync(
                input: encryptedStream,
                output: decryptedStream,
                subscriberCredential);

            // Then
            string actualString = Encoding.UTF8.GetString(ReadAllBytesFromStream(decryptedStream));
            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}