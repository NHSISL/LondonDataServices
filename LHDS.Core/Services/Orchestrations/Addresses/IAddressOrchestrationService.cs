// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.Addresses
{
    public interface IAddressOrchestrationService
    {
        ValueTask BulkAddAddressesAsync(Stream input, string fileName);
        ValueTask SyncAddressesWithAssignAsync();
    }
}
