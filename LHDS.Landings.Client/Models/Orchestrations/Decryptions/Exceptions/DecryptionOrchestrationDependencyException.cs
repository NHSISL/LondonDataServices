// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Orchestrations.Decryptions.Exceptions
{
    public class DecryptionOrchestrationDependencyException : Xeption
    {
        public DecryptionOrchestrationDependencyException(Xeption innerException)
         : base(
                message: "Decryption orchestration dependency error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
