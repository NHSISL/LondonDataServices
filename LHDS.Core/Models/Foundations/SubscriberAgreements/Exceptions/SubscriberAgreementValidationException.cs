using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class SubscriberAgreementValidationException : Xeption
    {
        public SubscriberAgreementValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}