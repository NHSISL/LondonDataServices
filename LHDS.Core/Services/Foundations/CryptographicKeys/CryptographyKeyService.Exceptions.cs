// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyService
    {
        private delegate ValueTask<CryptographicKey> ReturningCryptographyKeyFunction();

        private async ValueTask<CryptographicKey> TryCatch(
            ReturningCryptographyKeyFunction returningCryptographyKeyFunction)
        {
            try
            {
                return await returningCryptographyKeyFunction();
            }
            catch (InvalidArgumentCryptographyKeyException invalidArgumentCryptographyKeyException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentCryptographyKeyException);
            }
            catch (NullBrokerCryptographyKeyException nullBrokerCryptographyKeyException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBrokerCryptographyKeyException);
            }
            catch (Exception exception)
            {
                var failedCryptographyKeyServiceException =
                    new FailedCryptographyKeyServiceException(
                        message: "Failed cryptography key service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedCryptographyKeyServiceException);
            }
        }

        private async ValueTask<CryptographyKeyValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var cryptographyKeyValidationException =
                new CryptographyKeyValidationException(
                    message: "Cryptography key validation errors occurred, please try again.",
                    innerException: exception);

            await loggingBroker.LogErrorAsync(cryptographyKeyValidationException);

            return cryptographyKeyValidationException;
        }

        private async ValueTask<CryptographyKeyServiceException> CreateAndLogServiceExceptionAsync(
           Xeption exception)
        {
            var generateKeysServiceException = new CryptographyKeyServiceException(
                message: "Cryptography key service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(generateKeysServiceException);

            return generateKeysServiceException;
        }
    }
}