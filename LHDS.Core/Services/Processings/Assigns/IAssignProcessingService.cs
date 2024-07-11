// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;

namespace LHDS.Core.Services.Processings.Assigns
{
    public interface IAssignProcessingService
    {
        ValueTask SyncAddressesAsync(List<Address> addresses);
        ValueTask<AssignAddress> MatchAddressAsync(string address);
    }
}
