// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class DecryptionOrchestrationDependencyValidationException : Xeption
    {
        public DecryptionOrchestrationDependencyValidationException(string message, Xeption? innerException)
         : base(message, innerException)
        { }
    }
}
