// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions
{
    public class TerminologyDetailOrchestrationDependencyException : Xeption
    {
        public TerminologyDetailOrchestrationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
