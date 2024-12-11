// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class EmisLandingOrchestrationDependencyException : Xeption
    {
        public EmisLandingOrchestrationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
