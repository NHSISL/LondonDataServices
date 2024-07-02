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
            bool useOfflineProvider = config.GetValue<bool>("UseOfflineProvider");
            var orderedProvider = providers.OrderBy(p => p.IsOfflineProvider);
            provider = orderedProvider.First(provider => provider.IsOfflineProvider == useOfflineProvider);
        }

        public async ValueTask<List<string>> GetListOfDownloadsToProcessAsync(Download download) =>
            await this.provider.GetListOfDocumentsToProcessAsync(download);

        public async ValueTask GetDownloadByFileNameAsync(Download download) =>
            await this.provider.GetDocumentByFileNameAsync(download);
    }
}
