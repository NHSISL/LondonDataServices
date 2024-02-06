using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions
{
    public class InvalidSubscriberAgreementException : Xeption
    {
        public InvalidSubscriberAgreementException(string message)
            : base(message)
        { }
    }
}