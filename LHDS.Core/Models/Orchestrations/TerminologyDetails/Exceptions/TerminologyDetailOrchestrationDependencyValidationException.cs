// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions
{
    public class TerminologyDetailOrchestrationDependencyValidationException : Xeption
    {
        public TerminologyDetailOrchestrationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
