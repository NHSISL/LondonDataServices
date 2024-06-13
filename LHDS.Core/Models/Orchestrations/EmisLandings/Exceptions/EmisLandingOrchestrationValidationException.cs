// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class EmisLandingOrchestrationValidationException : Xeption
    {
        public EmisLandingOrchestrationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
