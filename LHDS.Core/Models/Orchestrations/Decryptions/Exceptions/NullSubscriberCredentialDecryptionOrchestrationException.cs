// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class NullSubscriberCredentialDecryptionOrchestrationException : Xeption
    {
        public NullSubscriberCredentialDecryptionOrchestrationException(string message)
            : base(message)
        { }
    }
}