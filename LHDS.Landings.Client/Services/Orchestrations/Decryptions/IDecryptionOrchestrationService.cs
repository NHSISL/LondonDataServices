// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Landings.Client.Services.Orchestrations.Decryptions
{
    public interface IDecryptionOrchestrationService
    {
        ValueTask DecryptAsync(string fileName);
    }
}
