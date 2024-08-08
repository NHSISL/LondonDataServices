// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class FailedIngresOrchestrationServiceException : Xeption
    {
        public FailedIngresOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}