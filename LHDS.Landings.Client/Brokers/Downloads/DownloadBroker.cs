// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Providers.Downloads;

namespace LHDS.Landings.Client.Brokers.Downloads
{
    public class DownloadBroker : IDownloadBroker
    {
        private readonly IDownloadAbstractProvider downloadAbstractProvider;

        public DownloadBroker(IDownloadAbstractProvider downloadAbstractProvider)
        {
            this.downloadAbstractProvider = downloadAbstractProvider;
        }

        public ValueTask<List<Document>> GetListOfDocumentsToProcessAsync() =>
            this.downloadAbstractProvider.GetListOfDocumentsToProcessAsync();

        public ValueTask<Document> GetDocumentByFileNameAsync(string fileName) =>
            this.downloadAbstractProvider.GetDocumentByFileNameAsync(fileName);
    }
}
