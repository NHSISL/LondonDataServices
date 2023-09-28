// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class DownloadOrchestrationServiceException : Xeption
    {
        public DownloadOrchestrationServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}