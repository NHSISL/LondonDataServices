// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Extensions.Addresses
{
    public static class AddressExtension
    {
        public static string GetFormattedAddress(this Address address)
        {
            var addressComponents = new List<string>();

            if (!string.IsNullOrEmpty(address.OrganisationName))
                addressComponents.Add(address.OrganisationName);

            if (!string.IsNullOrEmpty(address.DepartmentName))
                addressComponents.Add(address.DepartmentName);

            if (!string.IsNullOrEmpty(address.SubBuildingName))
                addressComponents.Add(address.SubBuildingName);

            if (!string.IsNullOrEmpty(address.BuildingName))
                addressComponents.Add(address.BuildingName);

            if (!string.IsNullOrEmpty(address.BuildingNumber))
                addressComponents.Add(address.BuildingNumber);

            if (!string.IsNullOrEmpty(address.DependentThoroughfare))
                addressComponents.Add(address.DependentThoroughfare);

            if (!string.IsNullOrEmpty(address.Thoroughfare))
                addressComponents.Add(address.Thoroughfare);

            if (!string.IsNullOrEmpty(address.DoubleDependentLocality))
                addressComponents.Add(address.DoubleDependentLocality);

            if (!string.IsNullOrEmpty(address.DependentLocality))
                addressComponents.Add(address.DependentLocality);

            if (!string.IsNullOrEmpty(address.PostTown))
                addressComponents.Add(address.PostTown);

            if (!string.IsNullOrEmpty(address.PostCode))
                addressComponents.Add(address.PostCode);

            return string.Join(", ", addressComponents);
        }
    }
}
