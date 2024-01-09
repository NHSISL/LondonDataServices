// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Models.Controllers.Documents;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string documentsRelativeUrl = "api/documents";
        public async ValueTask PostDocumentAsync(DocumentsModel documentsModel) =>
            await this.apiFactoryClient.PostContentAsync(documentsRelativeUrl, documentsModel);

        public async ValueTask<Document> DeleteDocumentByFileNameAsync(DocumentsFileModel documentsFileModel) =>
            await this.apiFactoryClient.DeleteContentAsync<Document>($"{documentsRelativeUrl}/{documentsFileModel}");
    }
}
