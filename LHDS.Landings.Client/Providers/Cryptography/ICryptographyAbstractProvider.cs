// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Landings.Client.Providers.Cryptography
{
    public interface ICryptographyAbstractProvider
    {
        ValueTask<byte[]> EncryptAsync(byte[] data);
        ValueTask<byte[]> DecryptAsync(byte[] data);
    }
}
