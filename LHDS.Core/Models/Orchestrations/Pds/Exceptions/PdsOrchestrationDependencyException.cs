// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class DownloadOrchestrationDependencyException : Xeption
    {
        public DownloadOrchestrationDependencyException(Xeption innerException)
         : base(
                message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
