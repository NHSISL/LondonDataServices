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