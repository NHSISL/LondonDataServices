// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<ResolvedAddress> ResolvedAddresses { get; set; }

        public async ValueTask<ResolvedAddress> InsertResolvedAddressAsync(
            ResolvedAddress resolvedAddress,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(resolvedAddress, cancellationToken);

        public async ValueTask BulkInsertResolvedAddressesAsync(
            List<ResolvedAddress> resolvedAddresses,
            bool useTransaction = true,
            CancellationToken cancellationToken = default) =>
                await BulkInsertAsync(resolvedAddresses, useTransaction, cancellationToken);

        public async ValueTask<IQueryable<ResolvedAddress>> SelectAllResolvedAddressesAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<ResolvedAddress>(cancellationToken);

        public async ValueTask<ResolvedAddress> SelectResolvedAddressByIdAsync(
            Guid resolvedAddressId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<ResolvedAddress>(new object[] { resolvedAddressId }, cancellationToken);

        public async ValueTask<ResolvedAddress> UpdateResolvedAddressAsync(
            ResolvedAddress resolvedAddress,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(resolvedAddress, cancellationToken);

        public async ValueTask BulkUpdateResolvedAddressesAsync(
            List<ResolvedAddress> resolvedAddresses,
            bool useTransaction = true,
            CancellationToken cancellationToken = default) =>
                await BulkUpdateAsync(resolvedAddresses, useTransaction, cancellationToken);

        public async ValueTask<ResolvedAddress> DeleteResolvedAddressAsync(
            ResolvedAddress resolvedAddress,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(resolvedAddress, cancellationToken);
    }
}
