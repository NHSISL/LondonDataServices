// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class FailedOptOutOrchestrationServiceException : Xeption
    {
        public FailedOptOutOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException) 
        { }
    }
}