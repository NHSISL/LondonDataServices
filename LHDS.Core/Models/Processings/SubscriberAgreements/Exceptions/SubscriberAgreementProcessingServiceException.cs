// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions
{
    public class SubscriberAgreementProcessingServiceException : Xeption
    {
        public SubscriberAgreementProcessingServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
