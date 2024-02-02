// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressToMatch))]
        public async Task ShouldCalculateMacthingAddressComponents(
            List<KeyValuePair<string, string>> inputAddress,
            HashSet<AddressMatch> potentialMatches,
            int matchedComponents,
            bool matchingCoreComponents)
        {
            // given
            List<KeyValuePair<string, string>> incomingAddress = inputAddress;
            HashSet<AddressMatch> possibleAddresses = potentialMatches;

            // when
            HashSet<AddressMatch> actualAddressMatches = addressMatcherService
                .CalculateMacthingAddressComponents(incomingAddress, possibleAddresses);

            AddressMatch actualAddressMatch = actualAddressMatches.First();

            // then
            actualAddressMatch.MatchedComponents.Should().Be(matchedComponents);
            actualAddressMatch.MatchingCoreComponents.Should().Be(matchingCoreComponents);
        }
    }
}