// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DocumentsServiceTests
    {

        [Fact]
        public async Task ShouldRetrieveFileAsync()
        {
            // Given
            string randomFileName = GetRandomString();
            var blobContainerName = this.inMemoryConfiguration.GetValue<string>("blobContainerName");

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
            Document actualDocument = await this.documentService.RetrieveDocumentByFileNameAsync(randomDocument.FileName);

            // Then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.SelectByFileNameAsync(randomDocument.FileName, blobContainerName),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}