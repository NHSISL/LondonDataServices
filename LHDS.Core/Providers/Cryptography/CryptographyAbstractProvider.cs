// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Providers.Cryptography
{
    public class CryptographyAbstractProvider : ICryptographyAbstractProvider
    {
        private readonly ICryptographyProvider provider;

        public CryptographyAbstractProvider(ICryptographyProvider provider)
        {
            this.provider = provider;
        }

        [Obsolete]
        public async ValueTask<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            await this.provider.EncryptAsync(data, subscriberCredential);

        [Obsolete]
        public async ValueTask<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            await this.provider.DecryptAsync(data, subscriberCredential);

        public async ValueTask EncryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            await this.provider.EncryptAsync(input, output, subscriberCredential);

        public async ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            await this.provider.DecryptAsync(input, output, subscriberCredential);
    }
}
