// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class InvalidArgumentIngressOrchestrationException : Xeption
    {
        public InvalidArgumentIngressOrchestrationException(string message)
            : base(message)
        { }
    }
}