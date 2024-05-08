// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class OptOutOrchestrationServiceException : Xeption
    {
        public OptOutOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}