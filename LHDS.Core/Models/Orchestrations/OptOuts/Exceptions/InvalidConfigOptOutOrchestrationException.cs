// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class InvalidConfigOptOutOrchestrationException : Xeption
    {
        public InvalidConfigOptOutOrchestrationException()
            : base(message: "Invalid Configuration orchestration error, please correct the errors and try again.") { }
    }
}