// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class DecryptionOrchestrationValidationException : Xeption
    {
        public DecryptionOrchestrationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
