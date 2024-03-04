// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Xeptions;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyProcessingService
    {
        private delegate ValueTask<SubscriberCredential> ReturningSubscriberCredentialFunction();

        private async ValueTask<SubscriberCredential> TryCatch(
            ReturningSubscriberCredentialFunction returningSubscriberCredentialFunction)
        {
            try
            {
                return await returningSubscriberCredentialFunction();
            }
            catch (NullSubscriberCredentialCryptographicKeyProcessingException
                nullSubscriberCredentialCryptographicKeyProcessingException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialCryptographicKeyProcessingException);
            }
        }

        private CryptographicKeyValidationProcessingException CreateAndLogValidationException(Xeption exception)
        {
            var subscriberCredentialValidationCryptographicKeyProcessingException =
                new CryptographicKeyValidationProcessingException(
                    message: "Cryptography key processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberCredentialValidationCryptographicKeyProcessingException);

            return subscriberCredentialValidationCryptographicKeyProcessingException;
        }

    }
}
