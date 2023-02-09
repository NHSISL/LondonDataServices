// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class DownloadOrchestrationValidationException : Xeption
    {
        public DownloadOrchestrationValidationException(Xeption innerException)
            : base(message: "Download orchestration validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
