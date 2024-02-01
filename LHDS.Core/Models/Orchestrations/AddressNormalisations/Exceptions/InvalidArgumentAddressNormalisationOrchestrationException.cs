// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions
{
    public class InvalidArgumentAddressNormalisationOrchestrationException : Xeption
    {
        public InvalidArgumentAddressNormalisationOrchestrationException(string message)
            : base(message)
        { }
    }
}