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
        ValueTask MatchAddressDataFromStreamAsync(Stream input, string fileName);
        ValueTask MatchAddressDataAsync();
        ValueTask<Guid?> UploadResolvedAddressesAsync();

        [Obsolete]
        ValueTask AddDocumentAsync(byte[] data, string fileName, string container);

        [Obsolete]
        ValueTask RemoveDocumentByFileNameAsync(string fileName, string container);
    }
}
