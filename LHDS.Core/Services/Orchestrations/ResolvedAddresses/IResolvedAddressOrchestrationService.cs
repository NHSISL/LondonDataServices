// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public interface IResolvedAddressOrchestrationService
    {
        ValueTask<List<Address>> UploadResolvedAddressesAsync();
        ValueTask RemoveDocumentByFileNameAsync(string filename, string container);
        ValueTask<string> AddDocumentAsync(Document document, string container);

    }
}
