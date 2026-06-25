// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions
{
    public class AddressToUprnOrchestrationValidationException : Xeption
    {
        public AddressToUprnOrchestrationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
