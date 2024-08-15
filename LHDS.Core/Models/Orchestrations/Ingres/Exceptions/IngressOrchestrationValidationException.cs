// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class IngressOrchestrationValidationException : Xeption
    {
        public IngressOrchestrationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
