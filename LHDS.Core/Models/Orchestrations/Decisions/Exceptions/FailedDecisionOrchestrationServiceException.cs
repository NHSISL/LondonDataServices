// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    internal class FailedDecisionOrchestrationServiceException : Xeption
    {
        public FailedDecisionOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}