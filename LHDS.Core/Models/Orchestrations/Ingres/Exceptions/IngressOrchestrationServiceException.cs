// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class IngressOrchestrationServiceException : Xeption
    {
        public IngressOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}