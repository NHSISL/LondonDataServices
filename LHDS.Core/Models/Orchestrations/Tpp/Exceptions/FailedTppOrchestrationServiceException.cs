// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Tpp.Exceptions
{
    public class FailedTppOrchestrationServiceException : Xeption
    {
        public FailedTppOrchestrationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}