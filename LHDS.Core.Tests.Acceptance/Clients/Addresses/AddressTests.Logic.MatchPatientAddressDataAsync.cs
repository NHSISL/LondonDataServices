// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public async Task ShouldMatchPatientAddressDataAsync()
        {
            //Given
            DateTimeOffset randomDateTime = DateTime.UtcNow;
            string fileName = GetRandomString();
            int count = GetRandomNumber();
            List<dynamic> dynamicAddresses = GetDynamicRandomAddresses();
            List<Address> listAddresses = new List<Address>();
            StringBuilder addressesToResolve = new StringBuilder();
            addressesToResolve.AppendLine("UniqueReference, Postcode, Address");

            foreach (dynamic dynamicAddress in dynamicAddresses)
            {
                string user = GetRandomString();

                Address inputAddress = new Address
                {
                    Id = Guid.NewGuid(),
                    UPSN = dynamicAddress.UPSN,
                    UPRN = dynamicAddress.UPRN,
                    OrganisationName = dynamicAddress.OrganisationName,
                    DepartmentName = dynamicAddress.DepartmentName,
                    SubBuildingName = dynamicAddress.SubBuildingName,
                    BuildingName = dynamicAddress.BuildingName,
                    BuildingNumber = dynamicAddress.BuildingNumber,
                    DependentThoroughfare = dynamicAddress.DependentThoroughfare,
                    Thoroughfare = dynamicAddress.Thoroughfare,
                    DoubleDependentLocality = dynamicAddress.DoubleDependentLocality,
                    DependentLocality = dynamicAddress.DependentLocality,
                    PostTown = dynamicAddress.PostTown,
                    PostCode = dynamicAddress.PostCode,
                    PostalAddress = dynamicAddress.PostalAddress,
                    JsonPostalAddress = dynamicAddress.JsonPostalAddress,
                    CreatedBy = user,
                    UpdatedBy = user,
                    CreatedDate = randomDateTime,
                    UpdatedDate = randomDateTime
                };

                Address savedAddress = await this.addressService.AddAddressAsync(inputAddress);
                listAddresses.Add(savedAddress);
                addressesToResolve
                    .AppendLine($"{Guid.NewGuid()},{dynamicAddress.PostCode},\"{dynamicAddress.PostalAddress}\"");
            }

            string inputAddresses = addressesToResolve.ToString();
            byte[] inputData = Encoding.UTF8.GetBytes(inputAddresses);

            //When
            await this.addressClient.MatchPatientAddressDataAsync(inputData, fileName);

            //Then
            foreach (Address address in listAddresses)
            {
                ResolvedAddress? matchedResolvedAddress = 
                    this.resolvedAddressService.RetrieveAllResolvedAddresses()
                        .FirstOrDefault(resolvedAddress => resolvedAddress.PostalAddress == address.PostalAddress);

                if(matchedResolvedAddress != null)
                {
                    await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(matchedResolvedAddress.Id);
                }
                await this.addressService.RemoveAddressByIdAsync(address.Id);
            }
        }
    }
}
