// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Models.Controllers.Documents;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Documents
{
    public partial class DocumentsApiTests
    {
        [Fact(Skip = "Don't process for now")]
        public async Task ShouldPostDocumentAsync()
        {
            // Given
            var blobStorageSettings = this.apiBroker.configuration
                .GetSection("blobStorage").Get<BlobStorageSettings>();

            Document inputDocument = new Document
            {
                DocumentData = Encoding.UTF8.GetBytes(GetRandomString()),
                FileName = $"{GetRandomString()}.txt"
            };

            DocumentsModel documentsModel = new DocumentsModel
            {
                Document = inputDocument,
                Container = blobStorageSettings.BlobContainers.EmisLanding
            };

            // When
            await this.apiBroker.PostDocumentAsync(documentsModel);

            // Then
            await this.apiBroker.DeleteDocumentByFileNameAsync(
                fileName: inputDocument.FileName,
                container: documentsModel.Container);
        }


        [Fact]
        public async Task DELETE_ShouldPostDocumentAsync()
        {
            // Given

            // When
            await this.apiBroker.GetDocumentAsync();

            // Then

        }
    }
}