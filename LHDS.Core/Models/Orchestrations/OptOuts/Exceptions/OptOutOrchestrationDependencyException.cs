// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class OptOutOrchestrationDependencyException : Xeption
    {
        public OptOutOrchestrationDependencyException(string message, Xeption? innerException)
         : base(message,innerException)
        { }
    }
}
