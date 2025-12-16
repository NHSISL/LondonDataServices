// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    internal class DecisionOrchestrationDependencyException : Xeption
    {
        public DecisionOrchestrationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
