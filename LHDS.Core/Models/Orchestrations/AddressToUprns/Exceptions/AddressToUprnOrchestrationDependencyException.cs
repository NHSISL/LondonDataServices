// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions
{
    public class AddressToUprnOrchestrationDependencyException : Xeption
    {
        public AddressToUprnOrchestrationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
