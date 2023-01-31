// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Landings.Client.Providers.Decryptions
{
    public class DecryptionAbstractProvider : IDecryptionAbstractProvider
    {
        private readonly IDecryptionProvider provider;

        public DecryptionAbstractProvider(IDecryptionProvider provider)
        {
            this.provider = provider;
        }

        public async ValueTask<byte[]> DecryptAsync(byte[] data) =>
            await this.provider.DecryptAsync(data);
    }
}
