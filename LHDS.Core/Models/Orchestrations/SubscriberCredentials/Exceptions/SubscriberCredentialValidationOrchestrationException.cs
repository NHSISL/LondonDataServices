// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions
{
    public class SubscriberCredentialValidationOrchestrationException : Xeption
    {
        public SubscriberCredentialValidationOrchestrationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
