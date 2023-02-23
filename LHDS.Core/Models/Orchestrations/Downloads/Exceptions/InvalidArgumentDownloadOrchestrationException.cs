// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Downloads.Exceptions
{
    public class InvalidArgumentDownloadOrchestrationException : Xeption
    {
        public InvalidArgumentDownloadOrchestrationException()
            : base(message: "Invalid download orchestration argument(s), please correct the errors and try again.")
        { }
    }
}