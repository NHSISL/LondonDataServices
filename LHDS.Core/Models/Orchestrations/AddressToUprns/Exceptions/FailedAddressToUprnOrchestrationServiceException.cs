// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions
{
    public class FailedAddressToUprnOrchestrationServiceException : Xeption
    {
        public FailedAddressToUprnOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
