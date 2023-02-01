// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Documents
{
    public partial class DocumentServiceTests
    {

        [Fact]
        public async Task ShouldAddFileAsync()
        {
            // Given
            string randomFileName = GetRandomString();
            var isDecrypted = false;

            var blobContainerName = this.inMemoryConfiguration
                .GetValue<string>("blobStorage:encryptedBlobContainerName");

            Document randomDocument = new Document
            {
                FileName = randomFileName,
                DocumentData = Encoding.ASCII.GetBytes(GetRandomString())
            };

            var stream = new MemoryStream(randomDocument.DocumentData);

            // When
            await this.documentService.AddDocumentAsync(randomDocument, isDecrypted);

            // Then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(randomDocument.FileName, It.IsAny<Stream>(), blobContainerName),
                Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}