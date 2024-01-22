// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions
{
    public class InvalidArgumentAddressExtractionOrchestrationException : Xeption
    {
        public InvalidArgumentAddressExtractionOrchestrationException(string message)
            : base(message)
        { }
    }
}