// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions
{
    public class TerminologyMetadataOrchestrationServiceException : Xeption
    {
        public TerminologyMetadataOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}