// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class SubscriberAgreementDependencyException : Xeption
    {
        public SubscriberAgreementDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}