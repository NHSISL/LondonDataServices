// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class NotFoundEmisLandingOrchestrationException : Xeption
    {
        public NotFoundEmisLandingOrchestrationException(string message)
            : base(message)
        { }
    }
}