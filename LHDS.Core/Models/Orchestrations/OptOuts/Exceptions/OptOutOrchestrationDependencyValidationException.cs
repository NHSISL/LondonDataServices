// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class OptOutOrchestrationDependencyValidationException : Xeption
    {
        public OptOutOrchestrationDependencyValidationException(Xeption innerException)
         : base(
                message: "Retrieve Opt Out Status orchestration dependency error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
