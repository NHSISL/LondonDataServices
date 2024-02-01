// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherServiceTests
    {
        [Theory]
        [MemberData(nameof(HouseNumbersWithCharacters))]
        public void ShouldRemoveNonDigitCharactersFromHouseNumber(List<KeyValuePair<string, string>> address)
        {
            // given
            IList<KeyValuePair<string, string>> inputAddress = address;

            IList<KeyValuePair<string, string>> expectedAddress =
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10"),
                    new KeyValuePair<string, string>("road", "Downing Street"),
                    new KeyValuePair<string, string>("suburb", "Westminster"),
                    new KeyValuePair<string, string>("city", "London"),
                    new KeyValuePair<string, string>("postcode", "SW1A2AA"),
                    new KeyValuePair<string, string>("country", "United Kingdom")
                };

            // when
            IList<KeyValuePair<string, string>> actualAddress = this.addressMatcherService
                .RemoveNonDigitCharactersFromHouseNumber(inputAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);
        }
    }
}

