// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class DecryptionOrchestrationValidationException : Xeption
    {
        public DecryptionOrchestrationValidationException(Xeption innerException)
            : base(
                message: "Decryption orchestration validation errors occurred, please try again.",
                innerException)
        { }
    }
}
