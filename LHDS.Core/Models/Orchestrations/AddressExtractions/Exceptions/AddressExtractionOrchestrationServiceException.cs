// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions
{
    public class AddressExtractionOrchestrationServiceException : Xeption
    {
        public AddressExtractionOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}