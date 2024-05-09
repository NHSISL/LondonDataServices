// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions
{
    public class AddressPersistenceOrchestrationServiceException : Xeption
    {
        public AddressPersistenceOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}