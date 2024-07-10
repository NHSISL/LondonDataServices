// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Addresses.Exceptions
{
    public class AddressOrchestrationDependencyValidationException : Xeption
    {
        public AddressOrchestrationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
