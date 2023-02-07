// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Orchestrations.Decryptions.Exceptions
{
    public class NullDecryptionOrchestrationFileNameException : Xeption
    {
        public NullDecryptionOrchestrationFileNameException()
            : base(message: "Filename is null.")
        { }
    }
}