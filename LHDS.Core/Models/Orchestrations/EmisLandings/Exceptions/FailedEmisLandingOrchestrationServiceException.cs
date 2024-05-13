// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class FailedEmisLandingOrchestrationServiceException : Xeption
    {
        public FailedEmisLandingOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}