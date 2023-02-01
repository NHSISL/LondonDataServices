// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Orchestrations.Downloads.Exceptions
{
    public class DownloadOrchestrationServiceException : Xeption
    {
        public DownloadOrchestrationServiceException(Exception innerException)
            : base(message: "Download orchestration service error occurred, contact support.", innerException)
        { }
    }
}