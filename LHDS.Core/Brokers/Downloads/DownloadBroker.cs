// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Providers.Downloads;

namespace LHDS.Core.Brokers.Downloads
{
    public class DownloadBroker : IDownloadBroker
    {
        private readonly IDownloadAbstractionProvider downloadAbstractProvider;

        public DownloadBroker(IDownloadAbstractionProvider downloadAbstractProvider)
        {
            this.downloadAbstractProvider = downloadAbstractProvider;
        }

        public ValueTask<List<string>> GetListOfDownloadsToProcessAsync(Download download) =>
            this.downloadAbstractProvider.GetListOfDownloadsToProcessAsync(download);

        public ValueTask GetDownloadByFileNameAsync(Download download) =>
            this.downloadAbstractProvider.GetDownloadByFileNameAsync(download);
    }
}
