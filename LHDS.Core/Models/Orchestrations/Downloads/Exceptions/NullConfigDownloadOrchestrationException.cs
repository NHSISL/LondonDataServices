// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class NullConfigDownloadOrchestrationException : Xeption
    {
        public NullConfigDownloadOrchestrationException(string message)
            : base(message)
        { }
    }
}
