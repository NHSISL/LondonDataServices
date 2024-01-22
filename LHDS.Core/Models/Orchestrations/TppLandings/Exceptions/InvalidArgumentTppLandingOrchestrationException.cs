// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TppLandings.Exceptions
{
    public class InvalidArgumentTppLandingOrchestrationException : Xeption
    {
        public InvalidArgumentTppLandingOrchestrationException(string message)
            : base(message)
        { }
    }
}