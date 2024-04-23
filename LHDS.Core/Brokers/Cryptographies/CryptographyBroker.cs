// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Cryptography;

namespace LHDS.Core.Brokers.Cryptographies
{
    public class CryptographyBroker : ICryptographyBroker
    {
        private readonly ICryptographyAbstractProvider decryptionAbstractProvider;

        public CryptographyBroker(ICryptographyAbstractProvider decryptionAbstractProvider)
        {
            this.decryptionAbstractProvider = decryptionAbstractProvider;
        }

        public ValueTask<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            decryptionAbstractProvider.EncryptAsync(data, subscriberCredential);

        public ValueTask<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            decryptionAbstractProvider.DecryptAsync(data, subscriberCredential);
    }
}
