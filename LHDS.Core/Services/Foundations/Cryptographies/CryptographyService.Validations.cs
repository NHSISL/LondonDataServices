// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;

namespace LHDS.Core.Services.Foundations.Cryptographies
{
    public partial class CryptographyService
    {
        private void ValidateData(byte[] data)
        {
            ValidateDataIsNotNull(data);
        }

        private static void ValidateDataIsNotNull(byte[] data)
        {
            if (data is null)
            {
                throw new NullCryptographyException(message: "Data is null.");
            }
        }
    }
}