using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class InvalidSubscriberAgreementReferenceException : Xeption
    {
        public InvalidSubscriberAgreementReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}