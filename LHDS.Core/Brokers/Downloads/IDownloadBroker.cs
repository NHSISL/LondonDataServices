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
        ValueTask<List<Download>> GetListOfDownloadsToProcessAsync(Download download);
        ValueTask<Download> GetDownloadByFileNameAsync(Download download);
    }
}
