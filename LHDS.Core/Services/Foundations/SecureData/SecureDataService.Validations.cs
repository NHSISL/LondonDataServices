// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public partial class SecureDataService
    {
        private void ValidateSecureDataOnAdd(SecureData secureData)
        {
            ValidateSecureDataIsNotNull(secureData);
        }

        private static void ValidateSecureDataIsNotNull(SecureData secureData)
        {
            if (secureData is null)
            {
                throw new NullSecureDataException(message: "Secure data is null.");
            }
        }

    }
}