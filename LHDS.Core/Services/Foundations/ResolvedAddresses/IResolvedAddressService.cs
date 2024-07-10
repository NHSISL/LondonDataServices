// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public interface IResolvedAddressService
    {
        ValueTask<ResolvedAddress> AddResolvedAddressAsync(ResolvedAddress resolvedAddress);
        ValueTask BulkAddResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses, string fileName);
        IQueryable<ResolvedAddress> RetrieveAllResolvedAddresses();
        ValueTask<ResolvedAddress> RetrieveResolvedAddressByIdAsync(Guid resolvedAddressId);
        ValueTask<ResolvedAddress> ModifyResolvedAddressAsync(ResolvedAddress resolvedAddress);
        ValueTask<ResolvedAddress> RemoveResolvedAddressByIdAsync(Guid resolvedAddressId);
    }
}