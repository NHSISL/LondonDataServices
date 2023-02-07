// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Models.Orchestrations.Decryptions.Exceptions;

namespace LHDS.Landings.Client.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationService
    {
        private static void ValidateFileNameIsNotNull(string fileName)
        {
            if (fileName is null)
            {
                throw new NullDecryptionOrchestrationFileNameException();
            }
        }
    }
}