// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    public class InvalidDecisionPollsDecisionOrchestrationException : Xeption
    {
        public InvalidDecisionPollsDecisionOrchestrationException(string message)
            : base(message)
        { }
    }
}
