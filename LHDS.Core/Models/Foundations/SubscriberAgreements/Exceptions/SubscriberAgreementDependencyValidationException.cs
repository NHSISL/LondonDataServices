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