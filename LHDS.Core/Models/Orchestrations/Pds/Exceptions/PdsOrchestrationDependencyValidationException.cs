// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pdss.Exceptions
{
    public class PdsOrchestrationDependencyValidationException : Xeption
    {
        public PdsOrchestrationDependencyValidationException(Xeption innerException)
         : base(
                message: "PDS orchestration dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
