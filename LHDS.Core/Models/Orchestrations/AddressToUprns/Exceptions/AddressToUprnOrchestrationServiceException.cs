// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions
{
    public class AddressToUprnOrchestrationServiceException : Xeption
    {
        public AddressToUprnOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
