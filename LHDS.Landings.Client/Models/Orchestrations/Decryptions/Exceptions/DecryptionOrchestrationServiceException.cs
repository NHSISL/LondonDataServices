// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Orchestrations.Decryptions.Exceptions
{
    public class DecryptionOrchestrationServiceException : Xeption
    {
        public DecryptionOrchestrationServiceException(Exception innerException)
            : base(message: "Decryption orchestration service error occurred, contact support.", innerException)
        { }
    }
}