// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressOrchestrationDependencyException : Xeption
    {
        public ResolvedAddressOrchestrationDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}
