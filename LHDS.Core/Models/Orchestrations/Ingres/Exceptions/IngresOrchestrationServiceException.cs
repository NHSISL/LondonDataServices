// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Ingres.Exceptions
{
    public class IngresOrchestrationServiceException : Xeption
    {
        public IngresOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}