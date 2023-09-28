// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class DownloadOrchestrationValidationException : Xeption
    {
        public DownloadOrchestrationValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
