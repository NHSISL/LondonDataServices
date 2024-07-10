// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask BulkInsertAddressesAsync(List<Address> address);
        ValueTask<Address> InsertAddressAsync(Address address);
        IQueryable<Address> SelectAllAddresses();
        ValueTask<Address> SelectAddressByIdAsync(Guid addressId);
        ValueTask<Address> UpdateAddressAsync(Address address);
        ValueTask BulkUpdateAddressesAsync(List<Address> address);
        ValueTask<Address> DeleteAddressAsync(Address address);
    }
}
