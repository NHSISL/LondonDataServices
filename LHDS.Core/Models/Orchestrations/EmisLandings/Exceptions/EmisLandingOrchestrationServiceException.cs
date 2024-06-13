// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions
{
    public class EmisLandingOrchestrationServiceException : Xeption
    {
        public EmisLandingOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}