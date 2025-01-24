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
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentCryptographyException nullCryptographyException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullCryptographyException);
            }
            catch (NullSubscriberCredentialCryptographyException nullSubscriberCredentialCryptographyException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialCryptographyException);
            }
            catch (Exception exception)
            {
                var failedCryptographyServiceException =
                    new FailedCryptographyServiceException(
                        message: "Failed cryptography service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedCryptographyServiceException);
            }
        }

        private async ValueTask<CryptographyValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var CryptographyValidationException =
                new CryptographyValidationException(
                    message: "Cryptography validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(CryptographyValidationException);

            return CryptographyValidationException;
        }

        private async ValueTask<CryptographyServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var decryptionServiceException = new CryptographyServiceException(
                message: "Cryptography service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(decryptionServiceException);

            return decryptionServiceException;
        }
    }
}