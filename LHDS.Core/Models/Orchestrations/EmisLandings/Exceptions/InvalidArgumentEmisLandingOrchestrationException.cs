// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class InvalidArgumentEmisLandingOrchestrationException : Xeption
    {
        public InvalidArgumentEmisLandingOrchestrationException(string message)
            : base(message)
        { }
    }
}