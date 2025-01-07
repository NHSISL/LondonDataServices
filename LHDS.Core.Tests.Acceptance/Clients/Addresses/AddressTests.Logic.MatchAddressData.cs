// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldMatchAddressDataAsync()
        {
            //Given
            DateTimeOffset randomDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            string fileName = GetRandomString();
            int count = GetRandomNumber();
            List<dynamic> dynamicAddresses = GetDynamicRandomAddresses();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: GetRandomNumber());
            List<ResolvedAddress> expectedResolvedAddresses = new List<ResolvedAddress>();
            List<Address> addedAddresses = new List<Address>();

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                await this.resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
                AssignAddress randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);

                this.wireMockServer
                    .Given(
                        Request
                            .Create()
                            .UsingGet()
                            .WithPath("/api/getinfo")
                            .WithParam("adrec", resolvedAddress.UnstructuredPostalAddress))
                    .RespondWith(
                        Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.OK)
                            .WithBodyAsJson(randomAssignAddress));

                Address randomAddress = CreateRandomAddress(randomDateTimeOffset, randomAssignAddress.BestMatch.UPRN);
                await this.addressService.AddAddressAsync(randomAddress);
                addedAddresses.Add(randomAddress);

                ResolvedAddress expectedResolvedAddress = MapOrdananceWithAssign(
                    resolvedAddress,
                    randomAssignAddress,
                    randomAddress);

                expectedResolvedAddresses.Add(expectedResolvedAddress);
            }

            //When
            await this.addressClient.MatchAddressDataAsync();

            //Then
            foreach (ResolvedAddress expectedResolvedAddress in expectedResolvedAddresses)
            {
                ResolvedAddress retrievedResolvedAddress =
                    this.resolvedAddressService.RetrieveAllResolvedAddresses()
                        .FirstOrDefault(resolvedAddress => resolvedAddress.Id == expectedResolvedAddress.Id);

                retrievedResolvedAddress.UPRN.Should().Be(expectedResolvedAddress.UPRN);
                retrievedResolvedAddress.UPSN.Should().Be(expectedResolvedAddress.UPSN);
                retrievedResolvedAddress.OrganisationName.Should().Be(expectedResolvedAddress.OrganisationName);
                retrievedResolvedAddress.DepartmentName.Should().Be(expectedResolvedAddress.DepartmentName);
                retrievedResolvedAddress.SubBuildingName.Should().Be(expectedResolvedAddress.SubBuildingName);
                retrievedResolvedAddress.BuildingName.Should().Be(expectedResolvedAddress.BuildingName);
                retrievedResolvedAddress.BuildingNumber.Should().Be(expectedResolvedAddress.BuildingNumber);

                retrievedResolvedAddress.DependentThoroughfare.Should().Be(
                    expectedResolvedAddress.DependentThoroughfare);

                retrievedResolvedAddress.Thoroughfare.Should().Be(expectedResolvedAddress.Thoroughfare);

                retrievedResolvedAddress.DoubleDependentLocality.Should().Be(
                    expectedResolvedAddress.DoubleDependentLocality);

                retrievedResolvedAddress.DependentLocality.Should().Be(expectedResolvedAddress.DependentLocality);
                retrievedResolvedAddress.PostTown.Should().Be(expectedResolvedAddress.PostTown);
                retrievedResolvedAddress.PostCode.Should().Be(expectedResolvedAddress.PostCode);
                retrievedResolvedAddress.UpdatedDate.Should().BeAfter(DateTimeOffset.Now.AddMinutes(-3));
                retrievedResolvedAddress.IsProcessed.Should().Be(true);
                await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(expectedResolvedAddress.Id);
            }

            foreach (Address addedAddress in addedAddresses)
            {
                await this.addressService.RemoveAddressByIdAsync(addedAddress.Id);
            }
        }
    }
}