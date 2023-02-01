// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldRetrieveFileAsync()
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

            Document expectedDocument = randomDocument;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.SelectByFileNameAsync(randomDocument.FileName, blobContainerName))
                    .ReturnsAsync(randomDocument.DocumentData);

            // When
            Document actualDocument =
                await this.documentService
                    .RetrieveDocumentByFileNameAsync(randomDocument.FileName, isDecrypted);

            // Then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.SelectByFileNameAsync(randomDocument.FileName, blobContainerName),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}