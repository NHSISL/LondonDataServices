// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions
{
    public class AddressExtractionValidationOrchestrationException : Xeption
    {
        public AddressExtractionValidationOrchestrationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
