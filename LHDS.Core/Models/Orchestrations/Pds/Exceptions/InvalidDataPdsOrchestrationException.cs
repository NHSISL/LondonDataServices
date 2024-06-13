// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class InvalidDataPdsOrchestrationException : Xeption
    {
        public InvalidDataPdsOrchestrationException(string message)
            : base(message)
        { }
    }
}
