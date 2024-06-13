// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions
{
    public class SubscriberCredentialOrchestrationServiceException : Xeption
    {
        public SubscriberCredentialOrchestrationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}