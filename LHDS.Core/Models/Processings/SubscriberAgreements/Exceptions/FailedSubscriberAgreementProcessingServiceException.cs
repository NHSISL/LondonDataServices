// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions
{
    public class FailedSubscriberAgreementProcessingServiceException : Xeption
    {
        public FailedSubscriberAgreementProcessingServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
