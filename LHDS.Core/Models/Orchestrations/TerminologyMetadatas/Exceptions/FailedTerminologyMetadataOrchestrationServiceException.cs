// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions
{
    public class FailedTerminologyMetadataOrchestrationServiceException : Xeption
    {
        public FailedTerminologyMetadataOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}