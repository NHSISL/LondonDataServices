// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressToMatch))]
        public async Task ShouldCalculateMacthingAddressComponents(
            List<KeyValuePair<string, string>> inputAddress,
            HashSet<AddressMatch> potentialMatches)
        {
            // given
            List<KeyValuePair<string, string>> incomingAddress = inputAddress;
            HashSet<AddressMatch> inputPossibleAddresses = potentialMatches;
            HashSet<AddressMatch> outputPossibleAddresses = inputPossibleAddresses;
            HashSet<AddressMatch> expectedAddressMatches = outputPossibleAddresses;

            addressMatcherServiceMock.Setup(x => x.CalculateMacthingAddressComponents(
                incomingAddress, inputPossibleAddresses))
                    .Returns(outputPossibleAddresses);

            // when
            HashSet<AddressMatch> actualAddressMatches = await addressMatcherProcessingService
                .CalculateMacthingAddressComponents(incomingAddress, outputPossibleAddresses);

            // then
            actualAddressMatches.Should().BeEquivalentTo(expectedAddressMatches);
        }
    }
}
