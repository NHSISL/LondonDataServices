// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class InvalidArgumentOptOutOrchestrationException : Xeption
    {
        public InvalidArgumentOptOutOrchestrationException(string message)
            : base(message)
        { }
    }
}