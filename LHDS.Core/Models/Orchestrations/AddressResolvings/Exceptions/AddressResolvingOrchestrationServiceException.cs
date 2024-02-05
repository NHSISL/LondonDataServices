// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions
{
    public class AddressResolvingOrchestrationServiceException : Xeption
    {
        public AddressResolvingOrchestrationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}