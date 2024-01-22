// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions
{
    public class InvalidArgumentAddressPersistanceOrchestrationException : Xeption
    {
        public InvalidArgumentAddressPersistanceOrchestrationException(string message)
            : base(message)
        { }
    }
}