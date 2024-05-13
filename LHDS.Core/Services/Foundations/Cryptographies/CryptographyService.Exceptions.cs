// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Cryptographies
{
    public partial class CryptographyService
    {
        private delegate Task<byte[]> ReturningCryptographyFunction();

        private async Task<byte[]> TryCatch(ReturningCryptographyFunction returningCryptographyFunction)
        {
            try
            {
                return await returningCryptographyFunction();
            }
            catch (NullDataCryptographyException nullCryptographyException)
            {
                throw CreateAndLogValidationException(nullCryptographyException);
            }
            catch (NullSubscriberCredentialCryptographyException nullSubscriberCredentialCryptographyException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialCryptographyException);
            }
            catch (Exception exception)
            {
                var failedCryptographyServiceException =
                    new FailedCryptographyServiceException(
                        message: "Failed cryptography service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedCryptographyServiceException);
            }
        }

        private CryptographyValidationException CreateAndLogValidationException(Xeption exception)
        {
            var CryptographyValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(CryptographyValidationException);

            return CryptographyValidationException;
        }

        private CryptographyServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var decryptionServiceException = new CryptographyServiceException(
                message: "Cryptography service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(decryptionServiceException);

            return decryptionServiceException;
        }
    }
}