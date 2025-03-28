// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class IngressOrchestrationDependencyValidationException : Xeption
    {
        public IngressOrchestrationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
