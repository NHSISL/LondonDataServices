// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyMetadata.Exceptions
{
    public class TerminologyMetadataOrchestrationDependencyException : Xeption
    {
        public TerminologyMetadataOrchestrationDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
