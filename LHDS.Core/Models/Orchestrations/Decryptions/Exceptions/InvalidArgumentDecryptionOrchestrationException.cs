// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class InvalidArgumentDecryptionOrchestrationException : Xeption
    {
        public InvalidArgumentDecryptionOrchestrationException(string message)
            : base(message)
        { }
    }
}