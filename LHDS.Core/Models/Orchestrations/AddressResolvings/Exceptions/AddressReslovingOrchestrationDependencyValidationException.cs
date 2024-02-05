// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions
{
    public class AddressResolvingOrchestrationDependencyValidationException : Xeption
    {
        public AddressResolvingOrchestrationDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
