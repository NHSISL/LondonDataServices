// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressOrchesrations.Exceptions
{
    public class AddressExtractionOrchestrationValidationException : Xeption
    {
        public AddressExtractionOrchestrationValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}
