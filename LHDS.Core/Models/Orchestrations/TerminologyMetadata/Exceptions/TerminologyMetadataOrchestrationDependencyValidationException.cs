// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyMetadata.Exceptions
{
    public class TerminologyMetadataOrchestrationDependencyValidationException : Xeption
    {
        public TerminologyMetadataOrchestrationDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
