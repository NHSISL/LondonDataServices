// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions;
using Xeptions;

namespace LHDS.Landings.Client.Services.Foundations.Decryptions
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
                    new FailedDecryptionServiceException(exception);

                throw CreateAndLogServiceException(failedDecryptionServiceException);
            }
        }

        private DecryptionValidationException CreateAndLogValidationException(Xeption exception)
        {
            var DecryptionValidationException =
                new DecryptionValidationException(exception);

            this.loggingBroker.LogError(DecryptionValidationException);

            return DecryptionValidationException;
        }

        private DecryptionServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var decryptionServiceException = new DecryptionServiceException(exception);
            this.loggingBroker.LogError(decryptionServiceException);

            return decryptionServiceException;
        }
    }
}