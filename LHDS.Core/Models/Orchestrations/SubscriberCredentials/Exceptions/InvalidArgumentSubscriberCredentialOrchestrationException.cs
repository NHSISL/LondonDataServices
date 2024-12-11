// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions
{
    public class InvalidArgumentSubscriberCredentialOrchestrationException : Xeption
    {
        public InvalidArgumentSubscriberCredentialOrchestrationException(string message)
            : base(message)
        { }
    }
}