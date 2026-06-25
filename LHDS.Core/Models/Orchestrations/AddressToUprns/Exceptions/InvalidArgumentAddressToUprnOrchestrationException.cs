// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions
{
    public class InvalidArgumentAddressToUprnOrchestrationException : Xeption
    {
        public InvalidArgumentAddressToUprnOrchestrationException(string message)
            : base(message)
        { }
    }
}
