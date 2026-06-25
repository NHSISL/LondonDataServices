// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions
{
    public class AddressToUprnOrchestrationDependencyValidationException : Xeption
    {
        public AddressToUprnOrchestrationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
