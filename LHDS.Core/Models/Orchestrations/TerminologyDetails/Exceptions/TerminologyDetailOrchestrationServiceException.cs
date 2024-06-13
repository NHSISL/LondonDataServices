// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions
{
    public class TerminologyDetailOrchestrationServiceException : Xeption
    {
        public TerminologyDetailOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException) 
        { }
    }
}