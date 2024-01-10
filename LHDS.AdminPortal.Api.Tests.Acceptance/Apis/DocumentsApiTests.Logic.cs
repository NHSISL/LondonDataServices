// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Models.Controllers.Documents;
using LHDS.Core.Models.Foundations.Documents;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Documents
{
    public partial class DocumentsApiTests
    {
        [Fact]
        public async Task ShouldPostDocumentAsync()
        {
            // Given
            Document randomDocument = CreateRandomDocument();
            Document inputDocument = randomDocument;
            Document expectedDocument = inputDocument;

            DocumentsModel documentsModel = new DocumentsModel
            {
                Document = inputDocument,
                Container = GetRandomString()
            };

            // When
            await this.apiBroker.PostDocumentAsync(documentsModel);

            // Then
            await this.apiBroker.DeleteDocumentByFileNameAsync(
                fileName: inputDocument.FileName,
                container: documentsModel.Container);
        }
    }
}