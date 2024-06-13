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
                throw CreateAndLogValidationException(invalidArgumentCryptographyKeyException);
            }
            catch (NullBrokerCryptographyKeyException nullBrokerCryptographyKeyException)
            {
                throw CreateAndLogValidationException(nullBrokerCryptographyKeyException);
            }
            catch (Exception exception)
            {
                var failedCryptographyKeyServiceException =
                    new FailedCryptographyKeyServiceException(
                        message: "Failed cryptography key service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedCryptographyKeyServiceException);
            }
        }

        private CryptographyKeyValidationException CreateAndLogValidationException(Xeption exception)
        {
            var cryptographyKeyValidationException =
                new CryptographyKeyValidationException(
                    message: "Cryptography key validation errors occurred, please try again.",
                    innerException: exception);

            loggingBroker.LogError(cryptographyKeyValidationException);

            return cryptographyKeyValidationException;
        }

        private CryptographyKeyServiceException CreateAndLogServiceException(
           Xeption exception)
        {
            var generateKeysServiceException = new CryptographyKeyServiceException(
                message: "Cryptography key service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(generateKeysServiceException);

            return generateKeysServiceException;
        }
    }
}