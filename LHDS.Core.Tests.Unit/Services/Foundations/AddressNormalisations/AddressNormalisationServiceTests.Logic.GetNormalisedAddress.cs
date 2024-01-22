// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        [Fact]
        public async Task ShouldGetNormalisedAddressAsync()
        {
            // given
            string inputAddress = "10 Downing Street, Westminster, London, SW1A2AA, United Kingdom";
            string[] expandedAddress = new List<string> { inputAddress.DeepClone() }.ToArray();

            List<KeyValuePair<string, string>> parsedAddress =
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10 Downing Street"),
                    new KeyValuePair<string, string>("road", "Downing Street"),
                    new KeyValuePair<string, string>("suburb", "Westminster"),
                    new KeyValuePair<string, string>("city", "London"),
                    new KeyValuePair<string, string>("postcode", "SW1A2AA"),
                    new KeyValuePair<string, string>("country", "United Kingdom")
                };

            var jsonAddress =
                "{" +
                "\"house_number\":\"10 Downing Street\"," +
                "\"road\":\"Downing Street\"," +
                "\"suburb\":\"Westminster\"," +
                "\"city\":\"London\"," +
                "\"postcode\":\"SW1A2AA\"," +
                "\"country\":\"United Kingdom\"" +
                "}";

            this.addressNormalisationBrokerMock.Setup(broker =>
                broker.ExpandAddressAsync(inputAddress))
                    .ReturnsAsync(expandedAddress);

            this.addressNormalisationBrokerMock.Setup(broker =>
                broker.ParseAddressAsync(expandedAddress.First()))
                    .ReturnsAsync(parsedAddress);

            AddressNormalisation expectedAddressNormalisation =
                new AddressNormalisation
                {
                    PostalAddress = expandedAddress[0],
                    JsonPostalAddress = jsonAddress
                };

            // when
            AddressNormalisation actualAddressNormalisation =
                await this.addressNormalisationService.GetNormalisedAddress(inputAddress);

            // then
            actualAddressNormalisation.Should().BeEquivalentTo(expectedAddressNormalisation);

            this.addressNormalisationBrokerMock.Verify(broker =>
                broker.ExpandAddressAsync(inputAddress),
                    Times.Once);

            this.addressNormalisationBrokerMock.Verify(broker =>
                broker.ParseAddressAsync(expandedAddress.First()),
                    Times.Once);

            this.addressNormalisationBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}