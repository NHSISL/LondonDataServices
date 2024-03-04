// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyService
    {
        private void ValidateInputs(string cryptographyType, string publicKeyComment)
        {
            ValidateCryptographyTypeIsNotNull(cryptographyType);
            ValidatePublicKeyCommentIsNotNull(publicKeyComment);
        }

        private static void ValidateCryptographyTypeIsNotNull(string cryptographyType)
        {
            if (cryptographyType is null)
            {
                throw new NullCryptographyTypeCryptographyKeyException(message: "Cryptography type is null.");
            }
        }

        private static void ValidatePublicKeyCommentIsNotNull(string publicKeyComment)
        {
            if (publicKeyComment is null)
            {
                throw new NullPublicKeyCommentCryptographyKeyException(message: "Public key comment is null.");
            }
        }
    }
}