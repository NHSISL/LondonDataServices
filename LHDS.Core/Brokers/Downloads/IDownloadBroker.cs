// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads;

namespace LHDS.Core.Brokers.Downloads
{
    public interface IDownloadBroker
    {
        ValueTask<List<string>> GetListOfDownloadsToProcessAsync(Download download);
        ValueTask GetDownloadByFileNameAsync(Download download);
    }
}
