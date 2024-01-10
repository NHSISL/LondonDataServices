// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Providers.Cryptography;

namespace LHDS.Core.Brokers.Decryptions
{
    public class CryptographyBroker : ICryptographyBroker
    {
        private readonly ICryptographyAbstractProvider decryptionAbstractProvider;

        public CryptographyBroker(ICryptographyAbstractProvider decryptionAbstractProvider)
        {
            this.decryptionAbstractProvider = decryptionAbstractProvider;
        }

        public ValueTask<byte[]> EncryptAsync(byte[] data) =>
            this.decryptionAbstractProvider.EncryptAsync(data);

        public ValueTask<byte[]> DecryptAsync(byte[] data) =>
            this.decryptionAbstractProvider.DecryptAsync(data);
    }
}
