// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class DownloadOrchestrationDependencyValidationException : Xeption
    {
        public DownloadOrchestrationDependencyValidationException(string message, Xeption innerException)
         : base(
                message: "Download orchestration dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
