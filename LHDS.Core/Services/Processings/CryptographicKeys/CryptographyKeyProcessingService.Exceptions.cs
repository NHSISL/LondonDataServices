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
                throw await CreateAndLogDependencyValidationExceptionAsync(cryptographyKeyValidationException);
            }
            catch (CryptographyKeyDependencyValidationException cryptographyKeyDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(cryptographyKeyDependencyValidationException);
            }
            catch (CryptographyKeyDependencyException cryptographyKeyDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(cryptographyKeyDependencyException);
            }
            catch (CryptographyKeyServiceException cryptographyKeyServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(cryptographyKeyServiceException);
            }
            catch (NullSubscriberCredentialCryptographicKeyProcessingException
                nullSubscriberCredentialCryptographicKeyProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialCryptographicKeyProcessingException);
            }
            catch (Exception exception)
            {
                var failedCryptographicKeyProcessingServiceException =
                    new FailedCryptographicKeyProcessingServiceException(
                        message: "Failed cryptography key processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedCryptographicKeyProcessingServiceException);
            }
        }

        private async ValueTask<CryptographicKeyValidationProcessingException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var subscriberCredentialValidationCryptographicKeyProcessingException =
                new CryptographicKeyValidationProcessingException(
                    message: "Cryptography key processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialValidationCryptographicKeyProcessingException);

            return subscriberCredentialValidationCryptographicKeyProcessingException;
        }

        private async ValueTask<CryptographicKeyProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var cryptographicKeyProcessingDependencyValidationException =
                new CryptographicKeyProcessingDependencyValidationException(
                    message: "Cryptography key processing dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(cryptographicKeyProcessingDependencyValidationException);

            return cryptographicKeyProcessingDependencyValidationException;
        }

        private async ValueTask<CryptographicKeyProcessingDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var cryptographicKeyProcessingDependencyException =
                new CryptographicKeyProcessingDependencyException(
                    message: "Cryptographic key processing dependency error occurred, fix the errors and try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(cryptographicKeyProcessingDependencyException);

            return cryptographicKeyProcessingDependencyException;
        }

        private async ValueTask<CryptographicKeyProcessingServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var cryptographicKeyProcessingServiceException = new
                CryptographicKeyProcessingServiceException(
                    message: "Cryptography key processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(cryptographicKeyProcessingServiceException);

            return cryptographicKeyProcessingServiceException;
        }
    }
}
