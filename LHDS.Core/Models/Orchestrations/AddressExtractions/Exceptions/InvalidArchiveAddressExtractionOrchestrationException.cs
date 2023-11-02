// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions
{
    public class InvalidArchiveAddressExtractionOrchestrationException : Xeption
    {
        public InvalidArchiveAddressExtractionOrchestrationException(string message)
            : base(message)
        { }
    }
}