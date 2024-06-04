// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public interface IAddressService
    {
        ValueTask BulkAddAddressesAsync(List<Address> addresses, string fileName);
        ValueTask<Address> AddAddressAsync(Address address);
        IQueryable<Address> RetrieveAllAddresses();
        ValueTask<Address> RetrieveAddressByIdAsync(Guid addressId);
        ValueTask<Address> ModifyAddressAsync(Address address);
        ValueTask<Address> RemoveAddressByIdAsync(Guid addressId);
        ValueTask<List<Address>> RetrieveAddressesByPostCodeAsync(string postCode);
    }
}