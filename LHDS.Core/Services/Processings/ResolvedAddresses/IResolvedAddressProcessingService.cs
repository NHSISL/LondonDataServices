// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Processings.ResolvedAddresses
{
    public interface IResolvedAddressProcessingService
    {
        ValueTask<ResolvedAddress> AddResolvedAddressAsync(ResolvedAddress resolvedAddress);
        IQueryable<ResolvedAddress> RetrieveAllResolvedAddresses();
        ValueTask<ResolvedAddress> RetrieveResolvedAddressByIdAsync(Guid resolvedAddressId);
        ValueTask<ResolvedAddress> RetrieveOrAddResolvedAddressAsync(ResolvedAddress resolvedAddress);
        ValueTask<ResolvedAddress> ModifyOrAddResolvedAddressAsync(ResolvedAddress resolvedAddress);
        ValueTask<ResolvedAddress> ModifyResolvedAddressAsync(ResolvedAddress resolvedAddress);
        ValueTask<ResolvedAddress> RemoveResolvedAddressByIdAsync(Guid resolvedAddressId);
    }
}
