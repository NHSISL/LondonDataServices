// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions
{
    public class SubscriberCredentialProcessingDependencyException : Xeption
    {
        public SubscriberCredentialProcessingDependencyException(string message, Xeption? innerException)
        : base(message, innerException)
        { }
    }
}
