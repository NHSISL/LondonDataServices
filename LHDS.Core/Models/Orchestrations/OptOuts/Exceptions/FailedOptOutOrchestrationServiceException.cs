// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class FailedOptOutOrchestrationServiceException : Xeption
    {
        public FailedOptOutOrchestrationServiceException(Exception innerException)
            : base(message: "Failed opt out orchestration service occurred, please contact support", innerException) { }
    }
}