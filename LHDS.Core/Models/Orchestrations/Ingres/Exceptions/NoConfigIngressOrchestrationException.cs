// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class NoConfigIngressOrchestrationException : Xeption
    {
        public NoConfigIngressOrchestrationException(string message)
            : base(message)
        { }
    }
}