// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions
{
    public class AddressResolvingOrchestrationDependencyException : Xeption
    {
        public AddressResolvingOrchestrationDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
