// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Landings.Client.Brokers.Decryptions
{
    public class DecryptionBroker : IDecryptionBroker
    {
        //private readonly IDecryptionAbstractProvider decryptionAbstractProvider;

        public ValueTask<byte[]> DecryptAsync(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
