// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<Address> Addresses { get; set; }

        public async ValueTask BulkInsertAddressesAsync(List<Address> addresses) =>
            await BulkInsertAsync(addresses);

        public async ValueTask<Address> InsertAddressAsync(Address address) =>
            await InsertAsync(address);

        public IQueryable<Address> SelectAllAddresses() => ReadAll<Address>();

        public async ValueTask<Address> SelectAddressByIdAsync(Guid addressId) =>
            await ReadAsync<Address>(addressId);

        public async ValueTask<Address> UpdateAddressAsync(Address address) =>
            await UpdateAsync(address);

        public async ValueTask<Address> DeleteAddressAsync(Address address) =>
            await DeleteAsync(address);
    }
}
