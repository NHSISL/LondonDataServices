// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions
{
    public class FailedAddressNormalisationOrchestrationServiceException : Xeption
    {
        public FailedAddressNormalisationOrchestrationServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
