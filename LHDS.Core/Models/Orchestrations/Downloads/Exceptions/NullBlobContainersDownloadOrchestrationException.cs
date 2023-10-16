// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class NullBlobContainersDownloadOrchestrationException : Xeption
    {
        public NullBlobContainersDownloadOrchestrationException(string message)
            : base(message)
        { }
    }
}