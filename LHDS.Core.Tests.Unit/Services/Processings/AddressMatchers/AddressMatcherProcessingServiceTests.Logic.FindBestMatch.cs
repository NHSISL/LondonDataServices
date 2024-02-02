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
        [Fact]
        public async Task ShouldFindSingleBestMatch()
        {
            // given
            HashSet<AddressMatch> inputAddressMacthes = CreateSingleBestMatch();
            AddressMatch expectedAddressMatch = inputAddressMacthes.FirstOrDefault();

            // when
            AddressMatch actualAddressMatch = await this.addressMatcherProcessingService
                .FindBestMacth(inputAddressMacthes);

            // then
            actualAddressMatch.Should().BeEquivalentTo(expectedAddressMatch);
        }

        [Fact]
        public async Task ShouldFindMultipleBestMatch()
        {
            // given
            HashSet<AddressMatch> inputAddressMacthes = CreateMultipleBestMatch();
            AddressMatch expectedAddressMatch = inputAddressMacthes.FirstOrDefault();

            // when
            AddressMatch actualAddressMatch = await this.addressMatcherProcessingService
                .FindBestMacth(inputAddressMacthes);

            // then
            actualAddressMatch.Should().BeEquivalentTo(expectedAddressMatch);
        }

    }
}
