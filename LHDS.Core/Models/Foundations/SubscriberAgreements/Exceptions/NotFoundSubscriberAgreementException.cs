// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class NotFoundSubscriberAgreementException : Xeption
    {
        public NotFoundSubscriberAgreementException(Guid subscriberAgreementId)
            : base(message: $"Couldn't find subscriberAgreement with subscriberAgreementId: {subscriberAgreementId}.")
        { }
    }
}