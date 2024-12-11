// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TppLandings.Exceptions
{
    public class TppLandingOrchestrationValidationException : Xeption
    {
        public TppLandingOrchestrationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
