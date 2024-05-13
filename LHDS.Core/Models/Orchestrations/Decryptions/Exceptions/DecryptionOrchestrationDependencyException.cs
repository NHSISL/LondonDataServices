// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class DecryptionOrchestrationDependencyException : Xeption
    {
        public DecryptionOrchestrationDependencyException(string message, Xeption? innerException)
         : base(message, innerException)
        { }
    }
}
