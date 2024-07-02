// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;

namespace LHDS.Core.Brokers.Hashing
{
    public interface IHashBroker
    {
        string GenerateSha256Hash(Stream? data);
        string GenerateMd5Hash(Stream? data);
    }
}
