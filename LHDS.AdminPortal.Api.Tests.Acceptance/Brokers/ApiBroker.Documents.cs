// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string documentsRelativeUrl = "api/documents";
        public async ValueTask PostDocumentAsync(Document document, string container) =>
            await this.apiFactoryClient.PostContentAsync(documentsRelativeUrl, document, container);

        public async ValueTask<Document> DeleteDocumentByFileNameAsync(string fileName, string container) =>
            await this.apiFactoryClient.DeleteContentAsync<Document>($"{documentsRelativeUrl}/{fileName}, {container}");
    }
}
