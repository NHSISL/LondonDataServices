// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.Decryptions
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
                throw new NullDecryptionException();
            }
        }
    }
}