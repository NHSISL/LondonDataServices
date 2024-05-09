// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions
{
    public class SubscriberCredentialDependencyOrchestrationException : Xeption
    {
        public SubscriberCredentialDependencyOrchestrationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
