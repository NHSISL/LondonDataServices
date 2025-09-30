// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    public class NullDecisionPollDecisionOrchestrationException : Xeption
    {
        public NullDecisionPollDecisionOrchestrationException(string message)
            : base(message)
        { }
    }
}
