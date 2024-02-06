using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService
    {
        private void ValidateSubscriberAgreementOnAdd(SubscriberAgreement subscriberAgreement)
        {
            ValidateSubscriberAgreementIsNotNull(subscriberAgreement);
        }

        private static void ValidateSubscriberAgreementIsNotNull(SubscriberAgreement subscriberAgreement)
        {
            if (subscriberAgreement is null)
            {
                throw new NullSubscriberAgreementException(message: "SubscriberAgreement is null.");
            }
        }
    }
}