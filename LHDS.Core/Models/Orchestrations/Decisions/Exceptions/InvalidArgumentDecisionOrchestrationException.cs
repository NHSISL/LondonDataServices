// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    public class InvalidArgumentDecisionOrchestrationException : Xeption
    {
        public InvalidArgumentDecisionOrchestrationException(string message)
            : base(message)
        { }
    }
}