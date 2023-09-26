// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class FailedDownloadOrchestrationServiceException : Xeption
    {
        public FailedDownloadOrchestrationServiceException(string message, Exception innerException)
            : base(message, innerException) 
        { }
    }
}