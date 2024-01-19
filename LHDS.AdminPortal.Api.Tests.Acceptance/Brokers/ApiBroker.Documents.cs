// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.AdminPortal.Api.Models.Controllers.Documents;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string documentsRelativeUrl = "api/documents";

        public async ValueTask GetDocumentAsync() =>
            await this.apiFactoryClient.GetContentAsync<object>($"{documentsRelativeUrl}");

        public async ValueTask PostDocumentAsync(DocumentsModel documentsModel) =>
            await this.apiFactoryClient.PostContentAsync(documentsRelativeUrl, documentsModel);

        public async ValueTask DeleteDocumentByFileNameAsync(string fileName, string container) =>
            await this.apiFactoryClient
                .DeleteContentAsync($"{documentsRelativeUrl}/{container}/{HttpUtility.UrlEncode(fileName)}");
    }
}
