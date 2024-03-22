// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Documents
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
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();

            // When
            byte[] encryptedData = await this.cryptographyProvider.EncryptAsync(randomBytes, randomSubscriberCredential);
            byte[] decryptedData = await this.cryptographyProvider.DecryptAsync(encryptedData, randomSubscriberCredential);
            string actualString = Encoding.UTF8.GetString(decryptedData);

            // Then
            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}