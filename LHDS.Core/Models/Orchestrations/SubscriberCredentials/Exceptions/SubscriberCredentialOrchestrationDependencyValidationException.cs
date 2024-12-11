// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions
{
    public class SubscriberCredentialOrchestrationDependencyValidationException : Xeption
    {
        public SubscriberCredentialOrchestrationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
