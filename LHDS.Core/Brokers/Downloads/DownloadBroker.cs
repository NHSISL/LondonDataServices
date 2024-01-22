// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
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

        public ValueTask<List<Document>> GetListOfDocumentsToProcessAsync() =>
            this.downloadAbstractProvider.GetListOfDocumentsToProcessAsync();

        public ValueTask<Document> GetDocumentByFileNameAsync(string fileName) =>
            this.downloadAbstractProvider.GetDocumentByFileNameAsync(fileName);
    }
}
