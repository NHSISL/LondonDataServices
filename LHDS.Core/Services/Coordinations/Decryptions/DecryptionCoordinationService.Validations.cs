// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;

namespace LHDS.Core.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationService
    {
        private void ValidateFileNameOnDecrypt(string fileName)
        {
            ValidateDataIsNotNull(fileName);
        }

        private static void ValidateDataIsNotNull(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new InvalidArgumentDecryptionCoordinationException(
                    message: "Invalid decryption coordination argument, please correct the errors and try again.");
            }
        }
    }
}