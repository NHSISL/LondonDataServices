// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Providers.Downloads
{
    public class FtpProvider : IDownloadProvider
    {
        public ValueTask<Document> GetDocumentByFileNameAsync(string fileName) =>
            throw new NotImplementedException();

        public ValueTask<List<Document>> GetListOfDocumentsToProcessAsync() =>
            throw new NotImplementedException();
    }
}
