// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class NullLandingConfigurationEmisLandingOrchestrationException : Xeption
    {
        public NullLandingConfigurationEmisLandingOrchestrationException(string message)
            : base(message)
        { }
    }
}