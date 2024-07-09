// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Addresses.Exceptions
{
    public class InvalidArgumentAddressOrchestrationException : Xeption
    {
        public InvalidArgumentAddressOrchestrationException(string message)
            : base(message)
        { }
    }
}