using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class SubscriberAgreementServiceException : Xeption
    {
        public SubscriberAgreementServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}