// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions
{
    public class AddressResolvingOrchestrationValidationException : Xeption
    {
        public AddressResolvingOrchestrationValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
