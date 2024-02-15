// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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

        public ValueTask<List<Download>> GetListOfDownloadsToProcessAsync(Download download) =>
            throw new NotImplementedException();

        public ValueTask<Download> GetDownloadByFileNameAsync(Download download) =>
            throw new NotImplementedException();
    }
}
