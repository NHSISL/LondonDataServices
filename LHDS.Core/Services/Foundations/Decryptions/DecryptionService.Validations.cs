// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Decryptions.Exceptions;

namespace LHDS.Core.Services.Foundations.Decryptions
{
    public partial class DecryptionService
    {
        private void ValidateDecryptionOnDecrypt(byte[] decryption)
        {
            ValidateDecryptionIsNotNull(decryption);
        }

        private static void ValidateDecryptionIsNotNull(byte[] Decryption)
        {
            if (Decryption is null)
            {
                throw new NullDecryptionException(message: "Decryption is null.");
            }
        }
    }
}