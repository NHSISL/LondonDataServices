// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions
{
    public class InvalidArgumentSubscriberAgreementProcessingException : Xeption
    {
        public InvalidArgumentSubscriberAgreementProcessingException(string message)
            : base(message)
        { }
    }
}