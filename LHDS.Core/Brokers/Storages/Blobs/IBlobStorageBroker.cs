// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Storages.Blobs
{
    public interface IBlobStorageBroker
    {
        ValueTask InsertFileAsync(string fileName, Stream stream);
        ValueTask<byte[]> SelectByFileNameAsync(string fileName);
        ValueTask DeleteFileAsync(string fileName);
        ValueTask<string> GetDownloadLinkAsync(string fileName, DateTimeOffset expiresOn);
    }
}
