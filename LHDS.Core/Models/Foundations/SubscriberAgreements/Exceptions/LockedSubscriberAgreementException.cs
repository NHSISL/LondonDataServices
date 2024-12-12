// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class LockedSubscriberAgreementException : Xeption
    {
        public LockedSubscriberAgreementException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}