// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class InvalidArgumentOptOutOrchestrationException : Xeption
    {
        public InvalidArgumentOptOutOrchestrationException()
            : base(message: "Invalid Retrieve Opt Out Status orchestration argument(s), please correct the errors and try again.") { }
    }
}