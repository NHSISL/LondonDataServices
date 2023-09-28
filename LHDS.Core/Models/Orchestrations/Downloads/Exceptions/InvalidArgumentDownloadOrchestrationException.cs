// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Reflection.Metadata;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class InvalidArgumentDownloadOrchestrationException : Xeption
    {
        public InvalidArgumentDownloadOrchestrationException(string message)
            : base(message) 
        { }
    }
}