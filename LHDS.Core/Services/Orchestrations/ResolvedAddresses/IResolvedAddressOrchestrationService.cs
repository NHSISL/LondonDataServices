// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Storages.StorageQueues;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public interface IResolvedAddressOrchestrationService
    {
        ValueTask UploadAddressesToResolveAsync(Stream input, string fileName);
        ValueTask MatchAddressDataAsync();
        ValueTask MatchAddressDataAsync(Payload<Guid> payload);
        ValueTask<List<Guid>> ExportResolvedAddressesAsync();
    }
}
