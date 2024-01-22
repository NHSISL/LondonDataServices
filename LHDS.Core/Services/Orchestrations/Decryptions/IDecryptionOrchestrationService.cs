// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public interface IDecryptionOrchestrationService
    {
        ValueTask<string> DecryptAsync(string fileName);
    }
}
