// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingService
    {
        private delegate ValueTask<SubscriberCredential> ReturningSubscriberCredentialFunction();

        private async ValueTask<SubscriberCredential> TryCatch(
            ReturningSubscriberCredentialFunction returningSubscriberCredentialFunction)
        {
            try
            {
                return await returningSubscriberCredentialFunction();
            }
            catch (NullSubscriberCredentialException nullSubscriberCredentialException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialException);
            }
            catch (InvalidSubscriberCredentialException invalidSubscriberCredentialException)
            {
                throw CreateAndLogValidationException(invalidSubscriberCredentialException);
            }
        }

        private SubscriberCredentialValidationException CreateAndLogValidationException(Xeption exception)
        {
            var subscriberCredentialValidationException = new SubscriberCredentialValidationException(
                message: "Subscriber credential validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(subscriberCredentialValidationException);

            return subscriberCredentialValidationException;
        }
    }
}