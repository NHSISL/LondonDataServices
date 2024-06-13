// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions
{
    public class FailedAddressExtractionOrchestrationServiceException : Xeption
    {
        public FailedAddressExtractionOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}