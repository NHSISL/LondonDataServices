// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decryptions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Decryptions
{
    public partial class DecryptionService
    {
        private delegate Task<byte[]> ReturningDecryptionFunction();

        private async Task<byte[]> TryCatch(ReturningDecryptionFunction returningDecryptionFunction)
        {
            try
            {
                return await returningDecryptionFunction();
            }
            catch (NullDecryptionException nullDecryptionException)
            {
                throw CreateAndLogValidationException(nullDecryptionException);
            }
            catch (Exception exception)
            {
                var failedDecryptionServiceException =
                    new FailedDecryptionServiceException(
                        message: "Failed decryption service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedDecryptionServiceException);
            }
        }

        private DecryptionValidationException CreateAndLogValidationException(Xeption exception)
        {
            var DecryptionValidationException =
                new DecryptionValidationException(
                    message: "Decryption validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(DecryptionValidationException);

            return DecryptionValidationException;
        }

        private DecryptionServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var decryptionServiceException = new DecryptionServiceException(
                message: "Decryption service error occurred, contact support.", 
                innerException: exception);

            this.loggingBroker.LogError(decryptionServiceException);

            return decryptionServiceException;
        }
    }
}