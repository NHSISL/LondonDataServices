// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decisions.Exceptions
{
    public class DecisionOrchestrationServiceException : Xeption
    {
        public DecisionOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}