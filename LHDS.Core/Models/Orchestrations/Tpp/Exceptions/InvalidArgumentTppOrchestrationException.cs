// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Tpp.Exceptions
{
    public class InvalidArgumentTppOrchestrationException : Xeption
    {
        public InvalidArgumentTppOrchestrationException(string message)
            : base(message)
        { }
    }
}