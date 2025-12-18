// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    public class DecisionOrchestrationValidationException : Xeption
    {
        public DecisionOrchestrationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
