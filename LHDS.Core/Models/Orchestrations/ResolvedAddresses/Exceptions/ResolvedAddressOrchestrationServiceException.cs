// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressOrchestrationServiceException : Xeption
    {
        public ResolvedAddressOrchestrationServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}
