// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Orchestrations.Decryptions.Exceptions
{
    public class InvalidArgumentDecryptionOrchestrationException : Xeption
    {
        public InvalidArgumentDecryptionOrchestrationException()
            : base(message: "Invalid decryption orchestration argument(s), please correct the errors and try again.")
        { }
    }
}