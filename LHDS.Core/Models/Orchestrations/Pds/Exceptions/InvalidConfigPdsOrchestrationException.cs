// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class InvalidConfigPdsOrchestrationException : Xeption
    {
        public InvalidConfigPdsOrchestrationException(string message)
            : base(message)
        { }
    }
}
