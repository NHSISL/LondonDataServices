// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class NotFoundDecryptionOrchestrationException : Xeption
    {
        public NotFoundDecryptionOrchestrationException(string message)
            : base(message)
        { }
    }
}