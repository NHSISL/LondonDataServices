// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class NullBlobContainersEmisLandingOrchestrationException : Xeption
    {
        public NullBlobContainersEmisLandingOrchestrationException(string message)
            : base(message)
        { }
    }
}