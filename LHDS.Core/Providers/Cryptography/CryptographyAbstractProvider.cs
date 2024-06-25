// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Providers.Cryptography
{
    public class CryptographyAbstractProvider : ICryptographyAbstractProvider
    {
        private readonly ICryptographyProvider provider;

        public CryptographyAbstractProvider(ICryptographyProvider provider) =>
            this.provider = provider;

        public async ValueTask EncryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            await provider.EncryptAsync(input, output, subscriberCredential);

        public async ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            await provider.DecryptAsync(input, output, subscriberCredential);
    }
}
