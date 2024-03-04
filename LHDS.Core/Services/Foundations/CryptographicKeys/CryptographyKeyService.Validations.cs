// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Brokers.CryptographyKeys;
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

        private void ValidateBrokerNotNull(ICryptographyKeyBroker broker)
        {
            ValidateCryptographyBrokerIsNotNull(broker);
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

        private static void ValidateCryptographyBrokerIsNotNull(ICryptographyKeyBroker broker)
        {
            if (broker is null)
            {
                throw new NullBrokerCryptographyKeyException(message: "Broker is null.");
            }
        }
    }
}