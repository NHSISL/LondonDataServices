// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
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

        [Obsolete]
        public ValueTask<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            decryptionAbstractProvider.EncryptAsync(data, subscriberCredential);

        [Obsolete]
        public ValueTask<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            decryptionAbstractProvider.DecryptAsync(data, subscriberCredential);

        public async ValueTask EncryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            await decryptionAbstractProvider.EncryptAsync(input, output, subscriberCredential);

        public async ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            await decryptionAbstractProvider.DecryptAsync(input, output, subscriberCredential);
    }
}
