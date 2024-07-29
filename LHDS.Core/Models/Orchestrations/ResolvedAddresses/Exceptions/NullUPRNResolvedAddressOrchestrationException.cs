// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions
{
    public class NullUPRNResolvedAddressOrchestrationException : Xeption
    {
        public NullUPRNResolvedAddressOrchestrationException(string message)
            : base(message)
        { }
    }
}
