// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace LHDS.Landings.Client.Brokers
{
    public interface IBlobStorageBroker
    {
        ValueTask InsertFileAsync(string fileName, Stream stream, string container);
    }
}
