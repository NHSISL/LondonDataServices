// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        [Fact]
        public Task ShouldParseAddress()
        {
            // given
            List<KeyValuePair<string, string>> inputAddress =
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10 Downing Street"),
                    new KeyValuePair<string, string>("road", "Downing Street"),
                    new KeyValuePair<string, string>("suburb", "Westminster"),
                    new KeyValuePair<string, string>("city", "London"),
                    new KeyValuePair<string, string>("postcode", "SW1A2AA"),
                    new KeyValuePair<string, string>("country", "United Kingdom")
                };

            var outputAddress =
                "{" +
                "\"house_number\":\"10 Downing Street\"," +
                "\"road\":\"Downing Street\"," +
                "\"suburb\":\"Westminster\"," +
                "\"city\":\"London\"," +
                "\"postcode\":\"SW1A2AA\"," +
                "\"country\":\"United Kingdom\"" +
                "}";

            var expectedAddress = outputAddress;

            var service = new AddressNormalisationService(
                addressNormalisationBroker: addressNormalisationBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);

            // when
            string actualAddress = service.ParseAddressToJson(inputAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);
            return Task.CompletedTask;
        }
    }
}