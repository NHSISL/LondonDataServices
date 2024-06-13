// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions
{
    public class FailedTerminologyDetailOrchestrationServiceException : Xeption
    {
        public FailedTerminologyDetailOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException) 
        { }
    }
}