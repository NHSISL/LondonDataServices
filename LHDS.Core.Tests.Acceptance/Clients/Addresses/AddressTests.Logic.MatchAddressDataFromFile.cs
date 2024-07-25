// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldMatchAddressDataFromFileAsync()
        {
            //Given
            DateTimeOffset randomDateTime = dateTimeBroker.GetCurrentDateTimeOffset();
            string fileName = GetRandomString();
            int count = GetRandomNumber();
            List<dynamic> dynamicAddresses = GetDynamicRandomAddresses();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 1);
            

            string inputAddresses = addressesToResolve.ToString();
            byte[] inputData = Encoding.UTF8.GetBytes(inputAddresses);

            //When
            await this.addressClient.MatchAddressDataAsync();

            //Then
            foreach (Address address in listAddresses)
            {
                ResolvedAddress? matchedResolvedAddress =
                    this.resolvedAddressService.RetrieveAllResolvedAddresses()
                        .FirstOrDefault(resolvedAddress => resolvedAddress.PostalAddress == address.PostalAddress);

                matchedResolvedAddress.MatchedUPRN = address.UPRN;
                matchedResolvedAddress.MatchedUPSN = address.UPSN;
                matchedResolvedAddress.MatchedOrganisationName = address.OrganisationName;
                matchedResolvedAddress.MatchedDepartmentName = address.DepartmentName;
                matchedResolvedAddress.MatchedSubBuildingName = address.SubBuildingName;
                matchedResolvedAddress.MatchedBuildingName = address.BuildingName;
                matchedResolvedAddress.MatchedBuildingNumber = address.BuildingNumber;
                matchedResolvedAddress.MatchedDependentThoroughfare = address.DependentThoroughfare;
                matchedResolvedAddress.MatchedThoroughfare = address.Thoroughfare;
                matchedResolvedAddress.MatchedDoubleDependentLocality = address.DoubleDependentLocality;
                matchedResolvedAddress.MatchedDependentLocality = address.DependentLocality;
                matchedResolvedAddress.MatchedPostTown = address.PostTown;
                matchedResolvedAddress.MatchedPostCode = address.PostCode;

                if (matchedResolvedAddress != null)
                {
                    await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(matchedResolvedAddress.Id);
                }

                await this.addressService.RemoveAddressByIdAsync(address.Id);
            }
        }
    }
}
