// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Providers.Cryptography
{
    public class CryptographyAbstractProvider : ICryptographyAbstractProvider
    {
        private readonly ICryptographyProvider provider;

        public CryptographyAbstractProvider(ICryptographyProvider provider)
        {
            this.provider = provider;
        }

        public async ValueTask<byte[]> EncryptAsync(byte[] data) =>
            await this.provider.EncryptAsync(data);

        public async ValueTask<byte[]> DecryptAsync(byte[] data) =>
            await this.provider.DecryptAsync(data);
    }
}
