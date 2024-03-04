// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyService
    {
        private delegate ValueTask<CryptographicKey> ReturningCryptographyKeyFunction();

        private async ValueTask<CryptographicKey> TryCatch(ReturningCryptographyKeyFunction returningCryptographyKeyFunction)
        {
            try
            {
                return await returningCryptographyKeyFunction();
            }
            catch (NullCryptographyTypeCryptographyKeyException nullCryptographyTypeCryptographyKeyException)
            {
                throw CreateAndLogValidationException(nullCryptographyTypeCryptographyKeyException);
            }
            catch (NullPublicKeyCommentCryptographyKeyException nullPublicKeyCommentCryptographyKeyException)
            {
                throw CreateAndLogValidationException(nullPublicKeyCommentCryptographyKeyException);
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
    }
}