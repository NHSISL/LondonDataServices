// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions
{
    public class NullSubscriberAgreementProcessingException : Xeption
    {
        public NullSubscriberAgreementProcessingException(string message)
            : base(message)
        { }
    }
}
