// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public interface IResolvedAddressOrchestrationService
    {
        ValueTask UploadAddressesToReslveAsync(Stream input, string fileName);
        ValueTask MatchAddressDataAsync();
        ValueTask<Guid?> ExportResolvedAddressesAsync();
    }
}
