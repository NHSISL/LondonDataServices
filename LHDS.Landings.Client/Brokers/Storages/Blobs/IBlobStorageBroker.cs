// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace LHDS.Landings.Client.Brokers.Storages.Blobs
{
    public interface IBlobStorageBroker
    {
        ValueTask InsertFileAsync(string fileName, Stream stream);
        ValueTask<byte[]> SelectByFileNameAsync(string fileName);
        ValueTask DeleteFileAsync(string fileName);
    }
}
