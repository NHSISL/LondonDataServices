// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Providers.Cryptography;

namespace LHDS.Landings.Client.Brokers.Decryptions
{
    public class DecryptionBroker : IDecryptionBroker
    {
        private readonly ICryptographyAbstractProvider decryptionAbstractProvider;

        public DecryptionBroker(ICryptographyAbstractProvider decryptionAbstractProvider)
        {
            this.decryptionAbstractProvider = decryptionAbstractProvider;
        }

        public ValueTask<byte[]> DecryptAsync(byte[] data) =>
            this.decryptionAbstractProvider.DecryptAsync(data);
    }
}
