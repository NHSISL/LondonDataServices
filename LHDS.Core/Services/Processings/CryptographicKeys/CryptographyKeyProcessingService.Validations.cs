// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyProcessingService
    {
        private static void ValidateSubscriberCredential(SubscriberCredential subscriberCredential)
        {
            ValidateSubscriberCredentialIsNotNull(subscriberCredential);
        }

        private static void ValidateSubscriberCredentialIsNotNull(SubscriberCredential subscriberCredential)
        {
            if (subscriberCredential == null)
            {
                throw new NullSubscriberCredentialCryptographicKeyProcessingException(
                    message: "Null subscriber credential processing exception, " +
                        "please correct the errors and try again.");
            }
        }
    }
}
