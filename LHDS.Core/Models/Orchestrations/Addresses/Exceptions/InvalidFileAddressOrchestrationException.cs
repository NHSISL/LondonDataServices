// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Addresses.Exceptions
{
    public class InvalidFileAddressOrchestrationException : Xeption
    {
        public InvalidFileAddressOrchestrationException(string message)
            : base(message)
        { }
    }
}