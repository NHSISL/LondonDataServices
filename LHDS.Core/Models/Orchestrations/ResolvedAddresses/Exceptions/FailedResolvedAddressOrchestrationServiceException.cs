// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions
{
    public class FailedResolvedAddressOrchestrationServiceException : Xeption
    {
        public FailedResolvedAddressOrchestrationServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
