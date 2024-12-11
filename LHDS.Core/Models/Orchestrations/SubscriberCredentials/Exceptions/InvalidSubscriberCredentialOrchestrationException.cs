// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions
{
    public class InvalidSubscriberCredentialOrchestrationException : Xeption
    {
        public InvalidSubscriberCredentialOrchestrationException(string message)
            : base(message)
        { }
    }
}