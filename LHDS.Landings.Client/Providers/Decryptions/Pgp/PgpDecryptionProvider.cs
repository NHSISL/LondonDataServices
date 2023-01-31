// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Landings.Client.Providers.Decryptions
{
    public class PgpDecryptionProvider : IDecryptionProvider
    {
        public ValueTask<byte[]> DecryptAsync(byte[] data) =>
            throw new NotImplementedException();
    }
}
