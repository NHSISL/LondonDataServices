// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Hashing
{
    public interface IHashBroker
    {
        ValueTask<string> GenerateSha256HashAsync(Stream? data, string? pepper = null);
        ValueTask<string> GenerateSha256HashAsync(string? data, string? pepper = null);
        ValueTask<string> GenerateMd5HashAsync(Stream? data);
    }
}
