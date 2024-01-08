// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Documents;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string documentsRelativeUrl = "api/documents";

        public async ValueTask PostDocumentAsync(Document document) =>
            await this.apiFactoryClient.PostContentAsync(documentsRelativeUrl, document);

        public async ValueTask DeleteDocumentByFileNameAsync(string fileName)
        {
            string encodedFileName = HttpUtility.UrlEncode(fileName);
            await this.apiFactoryClient.DeleteContentAsync<Document>($"{documentsRelativeUrl}/{encodedFileName}");
        }
    }
}
