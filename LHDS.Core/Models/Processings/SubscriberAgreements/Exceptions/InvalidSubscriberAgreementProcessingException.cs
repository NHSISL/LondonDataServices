// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions
{
    public class InvalidSubscriberAgreementProcessingException : Xeption
    {
        public InvalidSubscriberAgreementProcessingException(string message)
            : base(message)
        { }
    }
}