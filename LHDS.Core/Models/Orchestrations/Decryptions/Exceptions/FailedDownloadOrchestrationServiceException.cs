// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class FailedDecryptionOrchestrationServiceException : Xeption
    {
        public FailedDecryptionOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}