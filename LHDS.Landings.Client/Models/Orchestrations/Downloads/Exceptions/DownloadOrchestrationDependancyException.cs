// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Orchestrations.Downloads.Exceptions
{
    public class DownloadOrchestrationDependancyException : Xeption
    {
        public DownloadOrchestrationDependancyException(Xeption innerException)
         : base(
                message: "Download orchestration dependency error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
