// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions
{
    public class AddressNormalisationOrchestrationValidationException : Xeption
    {
        public AddressNormalisationOrchestrationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
