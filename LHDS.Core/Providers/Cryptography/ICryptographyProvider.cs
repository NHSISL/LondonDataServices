// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Providers.Cryptography
{
    public interface ICryptographyProvider
    {
        ValueTask<byte[]> EncryptAsync(byte[] data);
        ValueTask<byte[]> DecryptAsync(byte[] data);
    }
}
