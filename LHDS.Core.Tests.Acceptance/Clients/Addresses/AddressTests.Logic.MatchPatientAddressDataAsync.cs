// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact(Skip = "No longer used, will refactor out")]
        public async Task ShouldMatchPatientAddressDataAsync()
        {
            ////Given
            //DateTimeOffset randomDateTime = DateTime.UtcNow;
            //string fileName = GetRandomString();
            //int count = GetRandomNumber();
            //List<dynamic> dynamicAddresses = GetDynamicRandomAddresses();
            //List<Address> listAddresses = new List<Address>();
            //StringBuilder addressesToResolve = new StringBuilder();
            //addressesToResolve.AppendLine("UniqueReference, Postcode, Address");

            //foreach (dynamic dynamicAddress in dynamicAddresses)
            //{
            //    string user = GetRandomString();

            //    Address inputAddress = new Address
            //    {
            //        Id = Guid.NewGuid(),
            //        UPSN = dynamicAddress.UPSN,
            //        UPRN = dynamicAddress.UPRN,
            //        OrganisationName = dynamicAddress.OrganisationName,
            //        DepartmentName = dynamicAddress.DepartmentName,
            //        SubBuildingName = dynamicAddress.SubBuildingName,
            //        BuildingName = dynamicAddress.BuildingName,
            //        BuildingNumber = dynamicAddress.BuildingNumber,
            //        DependentThoroughfare = dynamicAddress.DependentThoroughfare,
            //        Thoroughfare = dynamicAddress.Thoroughfare,
            //        DoubleDependentLocality = dynamicAddress.DoubleDependentLocality,
            //        DependentLocality = dynamicAddress.DependentLocality,
            //        PostTown = dynamicAddress.PostTown,
            //        PostCode = dynamicAddress.PostCode,
            //        CreatedBy = user,
            //        UpdatedBy = user,
            //        CreatedDate = randomDateTime,
            //        UpdatedDate = randomDateTime
            //    };

            //    Address savedAddress = await this.addressService.AddAddressAsync(inputAddress);
            //    listAddresses.Add(savedAddress);
            //    addressesToResolve
            //        .AppendLine($"{Guid.NewGuid()},{dynamicAddress.PostCode},\"{dynamicAddress.PostalAddress}\"");
            //}

            //string inputAddresses = addressesToResolve.ToString();
            //byte[] inputData = Encoding.UTF8.GetBytes(inputAddresses);

            ////When
            //await this.addressClient.MatchPatientAddressDataAsync(inputData, fileName);

            ////Then
            //foreach (Address address in listAddresses)
            //{
            //    ResolvedAddress? matchedResolvedAddress =
            //        this.resolvedAddressService.RetrieveAllResolvedAddresses()
            //            .FirstOrDefault(resolvedAddress => resolvedAddress.PostalAddress == address.PostalAddress);

            //    matchedResolvedAddress.MatchedUPRN = address.UPRN;
            //    matchedResolvedAddress.MatchedUPSN = address.UPSN;
            //    matchedResolvedAddress.MatchedOrganisationName = address.OrganisationName;
            //    matchedResolvedAddress.MatchedDepartmentName = address.DepartmentName;
            //    matchedResolvedAddress.MatchedSubBuildingName = address.SubBuildingName;
            //    matchedResolvedAddress.MatchedBuildingName = address.BuildingName;
            //    matchedResolvedAddress.MatchedBuildingNumber = address.BuildingNumber;
            //    matchedResolvedAddress.MatchedDependentThoroughfare = address.DependentThoroughfare;
            //    matchedResolvedAddress.MatchedThoroughfare = address.Thoroughfare;
            //    matchedResolvedAddress.MatchedDoubleDependentLocality = address.DoubleDependentLocality;
            //    matchedResolvedAddress.MatchedDependentLocality = address.DependentLocality;
            //    matchedResolvedAddress.MatchedPostTown = address.PostTown;
            //    matchedResolvedAddress.MatchedPostCode = address.PostCode;

            //    if (matchedResolvedAddress != null)
            //    {
            //        await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(matchedResolvedAddress.Id);
            //    }

            //    await this.addressService.RemoveAddressByIdAsync(address.Id);
            //}
        }
    }
}
