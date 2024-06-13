// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TppLandings.Exceptions
{
    public class FailedTppLandingOrchestrationServiceException : Xeption
    {
        public FailedTppLandingOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}