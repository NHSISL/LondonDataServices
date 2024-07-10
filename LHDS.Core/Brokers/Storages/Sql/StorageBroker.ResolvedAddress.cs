// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<ResolvedAddress> ResolvedAddresses { get; set; }

        public async ValueTask<ResolvedAddress> InsertResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            await InsertAsync(resolvedAddress);

        public async ValueTask BulkInsertResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses) =>
            await BulkInsertAsync(resolvedAddresses);

        public IQueryable<ResolvedAddress> SelectAllResolvedAddresses() => ReadAll<ResolvedAddress>();

        public async ValueTask<ResolvedAddress> SelectResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            await ReadAsync<ResolvedAddress>(resolvedAddressId);

        public async ValueTask<ResolvedAddress> UpdateResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            await UpdateAsync(resolvedAddress);

        public async ValueTask BulkUpdateResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses) =>
            await BulkUpdateAsync(resolvedAddresses);

        public async ValueTask<ResolvedAddress> DeleteResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            await DeleteAsync(resolvedAddress);
    }
}
