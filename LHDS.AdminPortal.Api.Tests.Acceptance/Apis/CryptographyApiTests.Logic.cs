// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Cryptographies
{
    public partial class CryptographyApiTests
    {
        [Fact]
        public async Task ShouldEncryptAndDecryptAsync()
        {
            // given
            // TODO:  @Hassan -> Setup a subscriber agreement
            Guid subscriberAgreemnetId = Guid.NewGuid();
            string randomString = GetRandomString();
            byte[] inputBytes = Encoding.ASCII.GetBytes(randomString);
            string expectedString = randomString;

            //When
            byte[] encryptedData = await this.apiBroker
                .PostEncryptDataAsync(subscriberAgreemnetId, data: inputBytes);

            byte[] decryptedData = await this.apiBroker
                .PostDecryptDataAsync(subscriberAgreemnetId, data: encryptedData);

            string actualString = Encoding.ASCII.GetString(decryptedData);

            //Then
            actualString.Should().BeEquivalentTo(expectedString);
            // TODO:  @Hassan -> Remove the subscriber agreement
        }
    }
}
