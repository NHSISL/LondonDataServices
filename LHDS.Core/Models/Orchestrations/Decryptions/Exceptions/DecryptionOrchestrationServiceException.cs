// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class DecryptionOrchestrationServiceException : Xeption
    {
        public DecryptionOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}