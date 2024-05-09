// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class OptOutOrchestrationDependencyValidationException : Xeption
    {
        public OptOutOrchestrationDependencyValidationException(string message, Xeption? innerException)
         : base(message,innerException)
        { }
    }
}
