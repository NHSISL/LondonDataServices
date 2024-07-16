// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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

        public async ValueTask EncryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            await decryptionAbstractProvider.EncryptAsync(input, output, subscriberCredential);

        public async ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            await decryptionAbstractProvider.DecryptAsync(input, output, subscriberCredential);
    }
}
