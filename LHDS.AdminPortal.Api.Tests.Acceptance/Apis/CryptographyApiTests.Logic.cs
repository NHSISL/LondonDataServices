// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis
{
    public partial class CryptographyApiTests
    {
        [Fact]
        public async Task ShouldEncryptAndDecryptAsync()
        {
            // given
            string randomString = GetRandomString();
            byte[] inputBytes = Encoding.ASCII.GetBytes(randomString);
            string expectedString = randomString;

            //When
            byte[] encryptedData = await this.apiBroker.PostEncryptDataAsync(inputBytes);
            byte[] decryptedData = await this.apiBroker.PostDecryptDataAsync(encryptedData);
            string actualString = Encoding.ASCII.GetString(decryptedData);

            //Then
            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}
