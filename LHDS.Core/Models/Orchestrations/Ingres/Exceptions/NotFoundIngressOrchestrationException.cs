// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class NotFoundIngressOrchestrationException : Xeption
    {
        public NotFoundIngressOrchestrationException(string message)
            : base(message)
        { }
    }
}