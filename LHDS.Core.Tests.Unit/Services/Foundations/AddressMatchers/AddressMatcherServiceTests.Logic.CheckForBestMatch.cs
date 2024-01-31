// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherServiceTests
    {
        [Theory]
        [MemberData(nameof(CheckForBestMatch))]
        public void ShouldCheckForBestMatch(HashSet<AddressMatch> addresses, BestMatchEnum matchType)
        {
            // given
            HashSet<AddressMatch> inputAddresses = addresses;
            BestMatchEnum expectedMatchType = matchType;

            // when
            BestMatchEnum actualMatchType = this.addressMatcherService
                .CheckForBestMatch(macthedAddresses: inputAddresses);

            // then
            actualMatchType.Should().Be(expectedMatchType);
        }
    }
}

