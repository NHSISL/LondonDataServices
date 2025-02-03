// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions
{
    public class NullResolvedAddressOrchestrationException : Xeption
    {
        public NullResolvedAddressOrchestrationException(string message)
            : base(message)
        { }
    }
}
