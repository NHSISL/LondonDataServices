// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class FailedDecryptionOrchestrationServiceException : Xeption
    {
        public FailedDecryptionOrchestrationServiceException(Exception innerException)
            : base(message: "Failed Decryption orchestration service occurred, please contact support", innerException) { }
    }
}