// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Providers.Downloads
{
    public class DownloadAbstractionProvider : IDownloadAbstractionProvider
    {
        private readonly IDownloadProvider provider;

        public DownloadAbstractionProvider(IDownloadProvider provider)
        {
            this.provider = provider;
        }

        public async ValueTask<List<Document>> GetListOfDocumentsToProcessAsync() =>
            await this.provider.GetListOfDocumentsToProcessAsync();

        public async ValueTask<Document> GetDocumentByFileNameAsync(string fileName) =>
            await this.provider.GetDocumentByFileNameAsync(fileName);
    }
}
