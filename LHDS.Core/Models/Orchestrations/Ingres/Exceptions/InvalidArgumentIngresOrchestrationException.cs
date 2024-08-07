// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class InvalidArgumentIngresOrchestrationException : Xeption
    {
        public InvalidArgumentIngresOrchestrationException(string message)
            : base(message)
        { }
    }
}