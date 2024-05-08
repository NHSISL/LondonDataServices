using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class FailedSubscriberAgreementStorageException : Xeption
    {
        public FailedSubscriberAgreementStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}