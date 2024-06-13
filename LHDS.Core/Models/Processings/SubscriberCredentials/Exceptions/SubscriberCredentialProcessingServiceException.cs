// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions
{
    public class SubscriberCredentialProcessingServiceException : Xeption
    {
        public SubscriberCredentialProcessingServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
