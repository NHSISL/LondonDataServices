// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationService
    {
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentDecryptionCoordinationException invalidArgumentDecryptionCoordinationException)
            {
                throw CreateAndLogValidationException(invalidArgumentDecryptionCoordinationException);
            }
        }

        private DecryptionCoordinationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var decryptionCoordinationValidationException =
                new DecryptionCoordinationValidationException(
                    message: "Decryption coordination validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(decryptionCoordinationValidationException);

            return decryptionCoordinationValidationException;
        }
    }
}