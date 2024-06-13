// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions
{
    public class InvalidArgumentAddressPersistenceOrchestrationException : Xeption
    {
        public InvalidArgumentAddressPersistenceOrchestrationException(string message)
            : base(message)
        { }
    }
}