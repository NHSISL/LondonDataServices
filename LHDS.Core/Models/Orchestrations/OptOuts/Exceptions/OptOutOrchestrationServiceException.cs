// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class OptOutOrchestrationServiceException : Xeption
    {
        public OptOutOrchestrationServiceException(Exception innerException)
            : base(message: "Opt Out orchestration service error occurred, contact support.", innerException) { }
    }
}