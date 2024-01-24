// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(UncleanedAddressString))]
        public void ShouldCleanAddress(string uncleanedAddressString)
        {
            // given
            string uncleanedAddress = uncleanedAddressString;
            string expectedCleanedAddress = "a, a a, a";

            // when
            string actualAddress = this.addressMatcherProcessingService.CleanAddress(uncleanedAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedCleanedAddress);
        }

        [Fact]
        public void ShouldCleanRandomAddress()
        {
            // given
            int randomNumber = GetRandomNumber();
            string randomCleanAddress = GetRandomCleanAddressString(randomNumber);
            string expectedCleanedAddress = randomCleanAddress;
            string inputAddress = GetAddSpacesToString(randomCleanAddress);

            // when
            string actualAddress = this.addressMatcherProcessingService.CleanAddress(inputAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedCleanedAddress);
        }
    }
}
