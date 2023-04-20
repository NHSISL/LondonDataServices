// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class OptOutOrchestrationValidationException : Xeption
    {
        public OptOutOrchestrationValidationException(Xeption innerException)
            : base(
                message: "Retrieve OptOut Status orchestration validation errors occurred, please try again.",
                innerException)
        { }
    }
}