// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<ResolvedAddress> InsertResolvedAddressAsync(ResolvedAddress resolvedAddress);
        IQueryable<ResolvedAddress> SelectAllResolvedAddresses();
        ValueTask<ResolvedAddress> SelectResolvedAddressByIdAsync(Guid resolvedAddressId);
        ValueTask<ResolvedAddress> UpdateResolvedAddressAsync(ResolvedAddress resolvedAddress);
        ValueTask<ResolvedAddress> DeleteResolvedAddressAsync(ResolvedAddress resolvedAddress);
    }
}
