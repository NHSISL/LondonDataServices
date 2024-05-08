// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions
{
    public class FailedSubscriberCredentialProcessingServiceException : Xeption
    {
        public FailedSubscriberCredentialProcessingServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
