// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Foundations.Cryptographies
{
    public partial class CryptographyService
    {
        private void ValidateInputs(byte[] data, SubscriberCredential subscriberCredential)
        {
            ValidateDataIsNotNull(data);
            ValidateSubscriberCredentialsIsNotNull(subscriberCredential);
        }

        private static void ValidateDataIsNotNull(byte[] data)
        {
            if (data is null)
            {
                throw new NullDataCryptographyException(message: "Data is null.");
            }
        }

        private static void ValidateSubscriberCredentialsIsNotNull(SubscriberCredential subscriberCredential)
        {
            if (subscriberCredential is null)
            {
                throw new NullSubscriberCredentialCryptographyException(message: "Subscriber credential is null.");
            }
        }
    }
}