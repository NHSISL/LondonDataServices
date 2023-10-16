// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class NullLandingConfigurationDownloadOrchestrationException : Xeption
    {
        public NullLandingConfigurationDownloadOrchestrationException(string message)
            : base(message)
        { }
    }
}