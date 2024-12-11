// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class SubscriberAgreementDependencyValidationException : Xeption
    {
        public SubscriberAgreementDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}