// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class DownloadOrchestrationDependencyException : Xeption
    {
        public DownloadOrchestrationDependencyException(string message, Xeption innerException)
         : base(message, innerException)
        { }
    }
}
