// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class PdsOrchestrationDependencyException : Xeption
    {
        public PdsOrchestrationDependencyException(Xeption innerException)
         : base(
                message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
