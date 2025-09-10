// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    public class NullBlobContainersDecisionOrchestrationException : Xeption
    {
        public NullBlobContainersDecisionOrchestrationException(string message)
            : base(message)
        { }
    }
}
