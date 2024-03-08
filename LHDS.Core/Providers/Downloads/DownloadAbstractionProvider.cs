// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Providers.Downloads
{
    public class DownloadAbstractionProvider : IDownloadAbstractionProvider
    {
        private readonly IDownloadProvider provider;

        public DownloadAbstractionProvider(IEnumerable<IDownloadProvider> providers, IConfiguration config)
        {
            bool isMock = config.GetValue<bool>("IsMock");
            provider = providers.First(provider => provider.IsMock == isMock);
        }

        public async ValueTask<List<string>> GetListOfDownloadsToProcessAsync(Download download) =>
            await this.provider.GetListOfDocumentsToProcessAsync(download);

        public async ValueTask<Download> GetDownloadByFileNameAsync(Download download) =>
            await this.provider.GetDocumentByFileNameAsync(download);
    }
}
