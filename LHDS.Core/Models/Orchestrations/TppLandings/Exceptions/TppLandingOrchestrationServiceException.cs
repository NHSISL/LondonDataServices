// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TppLandings.Exceptions
{
    public class TppLandingOrchestrationServiceException : Xeption
    {
        public TppLandingOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}