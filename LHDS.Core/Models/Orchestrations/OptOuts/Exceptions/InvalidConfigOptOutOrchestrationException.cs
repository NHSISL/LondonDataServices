// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class InvalidConfigOptOutOrchestrationException : Xeption
    {
        public InvalidConfigOptOutOrchestrationException(string message)
            : base(message)
        { }
    }
}