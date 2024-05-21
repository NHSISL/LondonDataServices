// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public interface IResolvedAddressOrchestrationService
    {
        ValueTask<Guid?> UploadResolvedAddressesAsync();
        ValueTask AddDocumentAsync(byte[] data, string fileName, string container);
        ValueTask RemoveDocumentByFileNameAsync(string fileName, string container);
    }
}
