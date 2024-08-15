// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class FailedIngressOrchestrationServiceException : Xeption
    {
        public FailedIngressOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}