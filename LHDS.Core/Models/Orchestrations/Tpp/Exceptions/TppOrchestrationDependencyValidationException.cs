// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Tpp.Exceptions
{
    public class TppOrchestrationDependencyValidationException : Xeption
    {
        public TppOrchestrationDependencyValidationException(string message, Xeption innerException)
         : base(
                message: "Tpp Orchestration dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
