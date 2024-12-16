// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class NullSubscriberCredentialEmisLandingOrchestrationException : Xeption
    {
        public NullSubscriberCredentialEmisLandingOrchestrationException(string message)
            : base(message)
        { }
    }
}