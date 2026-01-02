// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    internal class DecisionOrchestrationDependencyValidationException : Xeption
    {
        public DecisionOrchestrationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}