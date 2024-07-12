// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Addresses.Exceptions
{
    public class AddressOrchestrationServiceException : Xeption
    {
        public AddressOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}