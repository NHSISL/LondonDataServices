// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class FailedPdsOrchestrationServiceException : Xeption
    {
        public FailedPdsOrchestrationServiceException(Exception innerException)
            : base(message: "Failed PDS orchestration service occurred, please contact support", innerException) { }
    }
}