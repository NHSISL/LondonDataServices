// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class OptOutOrchestrationDependencyException : Xeption
    {
        public OptOutOrchestrationDependencyException(Xeption innerException)
         : base(
                message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
