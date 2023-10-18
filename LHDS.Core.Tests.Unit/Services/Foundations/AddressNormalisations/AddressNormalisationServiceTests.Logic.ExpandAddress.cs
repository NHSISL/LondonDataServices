// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressExpansionData))]
        public async Task ShouldExpandAddressAsync(string inputAddress)
        {
            // given
            string expectedAddress = "10 Downing Street, Westminster, London, SW1A2AA, United Kingdom";

            var service = new AddressNormalisationService(
                addressNormalisationBroker: this.addressNormalisationBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);

            // when
            string actualAddress = service.CleanupAddress(inputAddress);

            // then
            output.WriteLine($"Input Address:   {inputAddress}");
            output.WriteLine($"Actual Address:   {actualAddress}");
            output.WriteLine($"Expected Address: {expectedAddress}");
            actualAddress.Should().BeEquivalentTo(expectedAddress);
        }
    }
}