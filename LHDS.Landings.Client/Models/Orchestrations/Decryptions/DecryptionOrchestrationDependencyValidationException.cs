// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Orchestrations.Decryptions
{
    public class DecryptionOrchestrationDependencyValidationException : Xeption
    {
        public DecryptionOrchestrationDependencyValidationException(Xeption innerException)
         : base(
                message: "Decryption orchestration dependency error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
