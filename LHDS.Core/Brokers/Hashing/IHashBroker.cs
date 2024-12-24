// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Hashing
{
    public interface IHashBroker
    {
        //string GenerateSha256Hash(Stream? data);
        ValueTask<string> GenerateSha256HashAsync(Stream? data);
        string GenerateMd5Hash(Stream? data);
        ValueTask<string> GenerateMd5HashAsync(Stream? data);
    }
}
