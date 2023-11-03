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
        public Task ShouldExpandAddressAsync(string inputAddress)
        {
            // given
            string expectedAddress = "10 Downing Street, Westminster, London, SW1A2AA, United Kingdom";

            var service = new AddressNormalisationService(
                addressNormalisationBroker: addressNormalisationBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);

            // when
            string actualAddress = service.CleanupAddress(inputAddress);

            // then
            output.WriteLine($"Raw Address:                {inputAddress}");
            output.WriteLine($"Expected/ Cleaned Address:  {expectedAddress}");
            output.WriteLine($"Actual Address:             {actualAddress}");
            actualAddress.Should().BeEquivalentTo(expectedAddress);
            return Task.CompletedTask;
        }
    }
}