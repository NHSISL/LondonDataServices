// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class NullBlobContainersOptOutOrchestrationException : Xeption
    {
        public NullBlobContainersOptOutOrchestrationException(string message)
            : base(message)
        { }
    }
}
