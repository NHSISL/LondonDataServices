// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions
{
    public class FailedSubscriberCredentialOrchestrationServiceException : Xeption
    {
        public FailedSubscriberCredentialOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}