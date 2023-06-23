// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class NotFoundDownloadOrchestrationException : Xeption
    {
        public NotFoundDownloadOrchestrationException(string fileName)
            : base(message: $"Couldn't find download with file name: {fileName}.") { }
    }
}