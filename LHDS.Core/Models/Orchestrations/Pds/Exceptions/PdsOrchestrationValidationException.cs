// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class PdsOrchestrationValidationException : Xeption
    {
        public PdsOrchestrationValidationException(Xeption innerException)
            : base(
                message: "PDS orchestration validation errors occurred, please try again.",
                innerException)
        { }
    }
}
