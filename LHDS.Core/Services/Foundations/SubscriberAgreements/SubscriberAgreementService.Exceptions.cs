using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService
    {
        private delegate ValueTask<SubscriberAgreement> ReturningSubscriberAgreementFunction();

        private async ValueTask<SubscriberAgreement> TryCatch(ReturningSubscriberAgreementFunction returningSubscriberAgreementFunction)
        {
            try
            {
                return await returningSubscriberAgreementFunction();
            }
            catch (NullSubscriberAgreementException nullSubscriberAgreementException)
            {
                throw CreateAndLogValidationException(nullSubscriberAgreementException);
            }
            catch (InvalidSubscriberAgreementException invalidSubscriberAgreementException)
            {
                throw CreateAndLogValidationException(invalidSubscriberAgreementException);
            }
        }

        private SubscriberAgreementValidationException CreateAndLogValidationException(Xeption exception)
        {
            var subscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberAgreementValidationException);

            return subscriberAgreementValidationException;
        }
    }
}