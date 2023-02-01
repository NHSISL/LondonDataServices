// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Providers.Decryptions;

namespace LHDS.Landings.Client.Brokers.Decryptions
{
    public class DecryptionBroker : IDecryptionBroker
    {
        private readonly IDecryptionAbstractProvider decryptionAbstractProvider;

        public DecryptionBroker(IDecryptionAbstractProvider decryptionAbstractProvider)
        {
            this.decryptionAbstractProvider = decryptionAbstractProvider;
        }

        public ValueTask<byte[]> DecryptAsync(byte[] data) =>
            this.decryptionAbstractProvider.DecryptAsync(data);
    }
}
