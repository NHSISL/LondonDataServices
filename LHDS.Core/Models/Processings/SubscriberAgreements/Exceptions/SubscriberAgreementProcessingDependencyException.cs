// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions
{
    public class SubscriberAgreementProcessingDependencyException : Xeption
    {
        public SubscriberAgreementProcessingDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
