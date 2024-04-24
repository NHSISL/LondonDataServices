// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions
{
    public class InvalidArgumentResolvedAddressOrchestrationException : Xeption
    {
        public InvalidArgumentResolvedAddressOrchestrationException(string message)
            : base(message)
        { }
    }
}