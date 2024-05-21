// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.AddressMatchers.Exceptions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnFindBestMatchIfServiceErrorOccursAsync()
        {
            // given
            List<KeyValuePair<string, string>> someIncomingAddressComponents =
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10"),
                    new KeyValuePair<string, string>("road", "downing str"),
                    new KeyValuePair<string, string>("city_district", "westminster"),
                    new KeyValuePair<string, string>("city", "london"),
                    new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                    new KeyValuePair<string, string>("country", "uk")
                };

            HashSet<AddressMatch> somePossibleAddresses = new HashSet<AddressMatch>
            {
                new AddressMatch
                {
                    PostalAddress = "10 downing str westminster london sw1a2aa uk",
                    JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                        "{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                        "{\"Key\":\"city_district\",\"Value\":\"westminster\"}," +
                        "{\"Key\":\"city\",\"Value\":\"london\"}," +
                        "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}," +
                        "{\"Key\":\"country\",\"Value\":\"uk\"}]",
                    NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city_district", "westminster"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                        new KeyValuePair<string, string>("country", "uk")
                    },
                    UPRN = GetRandomString(),
                    UPSN = GetRandomString(),
                }
            };

            var serviceException = new Exception();

            addressMatcherServiceMock.Setup(x => x.CalculateMatchingAddressComponents(
                someIncomingAddressComponents, somePossibleAddresses))
                    .Throws(serviceException);

            var failedAddressMatcherProcessingServiceException =
                new FailedAddressMatcherProcessingServiceException(
                    message: "Failed address matcher processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressMatcherProcessingServiceException =
                new AddressMatcherProcessingServiceException(
                    message: "Address matcher processing service error occurred, please contact support.",
                    innerException: failedAddressMatcherProcessingServiceException);

            // when
            ValueTask<AddressMatch> findBestMacthTask =
                addressMatcherProcessingService.FindBestMatch(
                    matchedAddresses: somePossibleAddresses,
                    addressComponents: someIncomingAddressComponents);

            AddressMatcherProcessingServiceException actualAddressMatcherProcessingServiceException =
                await Assert.ThrowsAsync<AddressMatcherProcessingServiceException>(
                    findBestMacthTask.AsTask);

            // then
            actualAddressMatcherProcessingServiceException.Should()
                .BeEquivalentTo(expectedAddressMatcherProcessingServiceException);
        }
    }
}