using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class FailedSubscriberAgreementServiceException : Xeption
    {
        public FailedSubscriberAgreementServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}