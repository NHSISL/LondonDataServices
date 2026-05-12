// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<ResolvedAddress> InsertResolvedAddressAsync(
            ResolvedAddress resolvedAddress,
            CancellationToken cancellationToken = default);

        ValueTask BulkInsertResolvedAddressesAsync(
            List<ResolvedAddress> resolvedAddresses,
            bool useTransaction = true,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<ResolvedAddress>> SelectAllResolvedAddressesAsync(
            CancellationToken cancellationToken = default);

        ValueTask<ResolvedAddress> SelectResolvedAddressByIdAsync(
            Guid resolvedAddressId,
            CancellationToken cancellationToken = default);

        ValueTask<ResolvedAddress> UpdateResolvedAddressAsync(
            ResolvedAddress resolvedAddress,
            CancellationToken cancellationToken = default);

        ValueTask BulkUpdateResolvedAddressesAsync(
            List<ResolvedAddress> resolvedAddresses,
            bool useTransaction = true,
            CancellationToken cancellationToken = default);

        ValueTask<ResolvedAddress> DeleteResolvedAddressAsync(
            ResolvedAddress resolvedAddress,
            CancellationToken cancellationToken = default);
    }
}
