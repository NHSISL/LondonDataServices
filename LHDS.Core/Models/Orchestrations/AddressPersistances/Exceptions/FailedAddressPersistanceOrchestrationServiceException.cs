// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions
{
    public class FailedAddressPersistanceOrchestrationServiceException : Xeption
    {
        public FailedAddressPersistanceOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}