// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class NullBlobContainersDecryptionOrchestrationException : Xeption
    {
        public NullBlobContainersDecryptionOrchestrationException(string message)
            : base(message)
        { }
    }
}