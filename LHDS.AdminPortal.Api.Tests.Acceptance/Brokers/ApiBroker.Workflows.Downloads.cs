// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Documents;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string downloadsRelativeUrl = "api/workflows/documents";

        public async ValueTask<List<Document>> GetListOfDocumentsToProcessAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Document>>(documentsRelativeUrl);

        public async ValueTask<Document> GetDocumentByFileNameAsync(string fileName)
        {
            string encodedFileName = HttpUtility.UrlEncode(fileName);
            return await this.apiFactoryClient.GetContentAsync<Document>($"{documentsRelativeUrl}/{encodedFileName}");
        }
    }
}
