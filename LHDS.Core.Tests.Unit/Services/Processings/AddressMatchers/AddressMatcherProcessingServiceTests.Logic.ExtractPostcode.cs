// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(CleanedAddressTestData))]
        public void ShouldExtractCorrectPostcode(string cleanedAddressWithPostcodes, string expectedPostcodes)
        {
            // given
            string cleanedAddress = cleanedAddressWithPostcodes;
            string expectedCleanedAddress = expectedPostcodes;

            // when
            string actualAddress = this.addressMatcherProcessingService.ExtractPostCode(cleanedAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedCleanedAddress);
        }
    }
}
