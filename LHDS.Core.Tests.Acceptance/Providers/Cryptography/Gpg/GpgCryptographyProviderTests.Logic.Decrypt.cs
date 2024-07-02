// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Providers.Cryptography.Gpg
{
    public partial class GpgCryptographyProviderTests
    {
        [Fact]
        public async Task ShouldEncryptAndDecryptStringAsync()
        {
            // Given
            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            Stream randomStream = new MemoryStream(randomBytes);
            Stream encryptedStream = new MemoryStream();
            Stream decryptedStream = new MemoryStream();
            string expectedString = randomString;

            // When
            await this.cryptographyProvider.EncryptAsync(
                input: randomStream, 
                output: encryptedStream,
                subscriberCredential);

            await this.cryptographyProvider.DecryptAsync(
                input: encryptedStream, 
                output: decryptedStream,
                subscriberCredential);

            byte[] decryptedData = ReadAllBytesFromStream(decryptedStream);
            string actualString = Encoding.UTF8.GetString(decryptedData);

            // Then
            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}