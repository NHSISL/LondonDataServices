// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<ResolvedAddress> InsertResolvedAddressAsync(ResolvedAddress resolvedAddress);
        ValueTask BulkInsertResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses);
        IQueryable<ResolvedAddress> SelectAllResolvedAddresses();
        ValueTask<ResolvedAddress> SelectResolvedAddressByIdAsync(Guid resolvedAddressId);
        ValueTask<ResolvedAddress> UpdateResolvedAddressAsync(ResolvedAddress resolvedAddress);
        ValueTask BulkUpdateResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses);
        ValueTask<ResolvedAddress> DeleteResolvedAddressAsync(ResolvedAddress resolvedAddress);
    }
}
