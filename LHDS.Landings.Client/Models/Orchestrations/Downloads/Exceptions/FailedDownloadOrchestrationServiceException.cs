// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Orchestrations.Downloads.Exceptions
{
    public class FailedDownloadOrchestrationServiceException : Xeption
    {
        public FailedDownloadOrchestrationServiceException(Exception innerException)
            : base(message: "Failed download orchestration service occurred, please contact support", innerException)
        { }
    }
}