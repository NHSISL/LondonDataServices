// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TppLandings.Exceptions
{
    public class TppLandingOrchestrationDependencyValidationException : Xeption
    {
        public TppLandingOrchestrationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
