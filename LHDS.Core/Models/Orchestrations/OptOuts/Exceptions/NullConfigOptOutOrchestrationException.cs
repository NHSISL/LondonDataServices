// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class NullConfigOptOutOrchestrationException : Xeption
    {
        public NullConfigOptOutOrchestrationException(string message)
            : base(message)
        { }
    }
}
