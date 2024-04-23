// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public interface IResolvedAddressOrchestrationService
    {
        ValueTask<List<ResolvedAddress>> UploadResolvedAddressesAsync();
        ValueTask AddDocumentAsync(byte[] data, string fileName, string container);
        ValueTask RemoveDocumentByFileNameAsync(string fileName, string container);
    }
}
