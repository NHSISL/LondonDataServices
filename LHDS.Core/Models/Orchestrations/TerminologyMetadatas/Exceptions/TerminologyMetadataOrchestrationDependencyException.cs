// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions
{
    public class TerminologyMetadataOrchestrationDependencyException : Xeption
    {
        public TerminologyMetadataOrchestrationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
