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
            string expectedString = randomString;
            byte[] decryptedBytes;

            // When
            using (Stream inputStream = new MemoryStream(randomBytes))
            using (Stream encryptedStream = new MemoryStream())
            using (Stream decryptedStream = new MemoryStream())
            {
                await this.cryptographyProvider
                    .EncryptAsync(input: inputStream, output: encryptedStream, subscriberCredential);

                await this.cryptographyProvider
                    .DecryptAsync(input: encryptedStream, output: decryptedStream, subscriberCredential);

                decryptedBytes = ReadAllBytesFromStream(stream: decryptedStream);
            }

            // Then
            string actualString = Encoding.UTF8.GetString(decryptedBytes);
            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}