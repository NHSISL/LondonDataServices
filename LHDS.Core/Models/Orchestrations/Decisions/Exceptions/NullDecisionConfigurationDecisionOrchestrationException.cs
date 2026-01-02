// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    public class NullDecisionConfigurationDecisionOrchestrationException : Xeption
    {
        public NullDecisionConfigurationDecisionOrchestrationException(string message)
            : base(message)
        { }
    }
}
