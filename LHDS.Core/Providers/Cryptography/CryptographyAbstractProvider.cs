// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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

        public async ValueTask<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            await this.provider.EncryptAsync(data, subscriberCredential);

        public async ValueTask<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            await this.provider.DecryptAsync(data, subscriberCredential);
    }
}
