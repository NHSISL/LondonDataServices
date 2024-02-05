// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions
{
    public class InvalidArgumentAddressResolvingOrchestrationException : Xeption
    {
        public InvalidArgumentAddressResolvingOrchestrationException(string message)
            : base(message)
        { }
    }
}