// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class NullSubscriberAgreementException : Xeption
    {
        public NullSubscriberAgreementException(string message)
            : base(message)
        { }
    }
}