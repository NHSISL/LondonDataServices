// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Brokers.Hashing
{
    public interface IHashBroker
    {
        string GenerateSha256Hash(byte[] data);
        string GenerateMd5Hash(byte[] data);
    }
}
