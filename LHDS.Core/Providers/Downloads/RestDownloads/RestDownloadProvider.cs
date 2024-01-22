// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Providers.Downloads.RestDownloads
{
    public class RestDownloadProvider : IDownloadProvider
    {
        public RestDownloadProvider()
        {
            Name = "RestDownloadProvider";
            IsMock = false;
        }

        public string Name { get; private set; }
        public bool IsMock { get; private set; }

        public ValueTask<Document> GetDocumentByFileNameAsync(string fileName) =>
            throw new NotImplementedException();

        public ValueTask<List<Document>> GetListOfDocumentsToProcessAsync() =>
            throw new NotImplementedException();
    }
}
