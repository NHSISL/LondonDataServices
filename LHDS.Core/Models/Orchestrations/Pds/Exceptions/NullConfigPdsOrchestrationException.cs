// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class NullConfigPdsOrchestrationException : Xeption
    {
        public NullConfigPdsOrchestrationException(string message)
            : base(message)
        { }
    }
}
