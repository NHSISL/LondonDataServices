// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldCalculateMatchingAddressComponents(
            List<KeyValuePair<string, string>> inputAddress,
            HashSet<AddressMatch> potentialMatches)
        {
            // given
            List<KeyValuePair<string, string>> incomingAddress = inputAddress;
            HashSet<AddressMatch> inputPossibleAddresses = potentialMatches;
            HashSet<AddressMatch> outputPossibleAddresses = inputPossibleAddresses;
            HashSet<AddressMatch> expectedAddressMatches = outputPossibleAddresses;

            addressMatcherServiceMock.Setup(x => x.CalculateMatchingAddressComponents(
                incomingAddress, inputPossibleAddresses))
                    .Returns(outputPossibleAddresses);

            // when
            HashSet<AddressMatch> actualAddressMatches = await addressMatcherProcessingService
                .CalculateMatchingAddressComponents(incomingAddress, inputPossibleAddresses);

            AddressMatch actualAddressMatch = actualAddressMatches.First();

            // then
            actualAddressMatches.Should().BeEquivalentTo(expectedAddressMatches);
        }
    }
}
