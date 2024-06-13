// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Processings.CryptographicKeys.Exceptions;
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
            catch (CryptographyKeyValidationException cryptographyKeyValidationException)
            {
                throw CreateAndLogDependencyValidationException(cryptographyKeyValidationException);
            }
            catch (CryptographyKeyDependencyValidationException cryptographyKeyDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(cryptographyKeyDependencyValidationException);
            }
            catch (CryptographyKeyDependencyException cryptographyKeyDependencyException)
            {
                throw CreateAndLogDependencyException(cryptographyKeyDependencyException);
            }
            catch (CryptographyKeyServiceException cryptographyKeyServiceException)
            {
                throw CreateAndLogDependencyException(cryptographyKeyServiceException);
            }
            catch (NullSubscriberCredentialCryptographicKeyProcessingException
                nullSubscriberCredentialCryptographicKeyProcessingException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialCryptographicKeyProcessingException);
            }
            catch (Exception exception)
            {
                var failedCryptographicKeyProcessingServiceException =
                    new FailedCryptographicKeyProcessingServiceException(
                        message: "Failed cryptography key processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedCryptographicKeyProcessingServiceException);
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

        private CryptographicKeyProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var cryptographicKeyProcessingDependencyValidationException =
                new CryptographicKeyProcessingDependencyValidationException(
                    message: "Cryptography key processing dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(cryptographicKeyProcessingDependencyValidationException);

            return cryptographicKeyProcessingDependencyValidationException;
        }

        private CryptographicKeyProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var cryptographicKeyProcessingDependencyException =
                new CryptographicKeyProcessingDependencyException(
                    message: "Cryptographic key processing dependency error occurred, fix the errors and try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(cryptographicKeyProcessingDependencyException);

            throw cryptographicKeyProcessingDependencyException;
        }

        private CryptographicKeyProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var cryptographicKeyProcessingServiceException = new
                CryptographicKeyProcessingServiceException(
                    message: "Cryptography key processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(cryptographicKeyProcessingServiceException);

            return cryptographicKeyProcessingServiceException;
        }
    }
}
