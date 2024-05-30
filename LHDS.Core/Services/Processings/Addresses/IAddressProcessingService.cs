// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

// Ignore Spelling: Processings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Processings.Addresses
{
    public interface IAddressProcessingService
    {
        ValueTask BulkAddAddressesAsync(List<Address> addresses, string fileName);
        ValueTask<Address> AddAddressAsync(Address address);
        IQueryable<Address> RetrieveAllAddresses();
        ValueTask<Address> RetrieveAddressByIdAsync(Guid addressId);
        ValueTask<Address> RetrieveOrAddAddressAsync(Address address);
        ValueTask<Address> ModifyOrAddAddressAsync(Address address);
        ValueTask<Address> ModifyAddressAsync(Address address);
        ValueTask<Address> RemoveAddressByIdAsync(Guid addressId);
        ValueTask<List<Address>> RetrieveAddressesByPostCodeAsync(string postCode);
        ValueTask<bool> IsExactMatchForAddressAsync(string addressToMatch);
    }
}
