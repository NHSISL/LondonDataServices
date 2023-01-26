// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Providers.Downloads
{
    public class DownloadAbstractProvider
    {
        private readonly RestProvider provider;

        public DownloadAbstractProvider(RestProvider provider)
        {
            this.provider = provider;
        }

        public async ValueTask<List<Document>> GetListOfDocumentsToProcessAsync() =>
            await this.provider.GetListOfDocumentsToProcessAsync();

        public async ValueTask<Document> GetDocumentAsync() =>
            await this.provider.GetDocumentAsync();
    }
}
