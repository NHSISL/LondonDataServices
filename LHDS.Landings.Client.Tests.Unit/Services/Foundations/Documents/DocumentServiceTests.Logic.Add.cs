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
            var blobContainerName = this.inMemoryConfiguration.GetValue<string>("blobContainerName");

            Document randomDocument = new Document
            {
                FileName = randomFileName,
                DocumentStream = new MemoryStream(Encoding.ASCII.GetBytes(GetRandomString()))
            };

            // When
            await this.documentService.AddDocumentAsync(randomDocument);

            // Then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(randomDocument.FileName, randomDocument.DocumentStream, blobContainerName),
                Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}