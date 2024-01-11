// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Providers.Downloads
{
    public class DownloadAbstractionProvider : IDownloadAbstractionProvider
    {
        private readonly IDownloadProvider provider;

        public DownloadAbstractionProvider(List<IDownloadProvider> providers, IConfiguration config)
        {
            bool isMock = config.GetValue<bool>("IsMock");
            provider = providers.First(provider => provider.IsMock == isMock);
        }

        public async ValueTask<List<Document>> GetListOfDocumentsToProcessAsync() =>
            await this.provider.GetListOfDocumentsToProcessAsync();

        public async ValueTask<Document> GetDocumentByFileNameAsync(string fileName) =>
            await this.provider.GetDocumentByFileNameAsync(fileName);
    }
}
