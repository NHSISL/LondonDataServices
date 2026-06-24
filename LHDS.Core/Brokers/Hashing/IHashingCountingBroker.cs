// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;

namespace LHDS.Core.Brokers.Hashing
{
    public interface IHashingCountingBroker : IDisposable, IAsyncDisposable
    {
        long BytesRead { get; }
        string GetFinalHashHex();
        Stream AsStream();
    }
}
