// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Landings.Client.Providers.Decryptions
{
    public interface IDecryptionProvider
    {
        ValueTask<byte[]> DecryptAsync(byte[] data);
    }
}
