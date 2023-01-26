// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Documents;
public partial class DocumentServiceTests
{
    [Fact]
    public async Task ShouldDeleteFileAsync()
    {
        // Given
        string randomFileName = GetRandomString();
        var blobContainerName = this.inMemoryConfiguration.GetValue<string>("blobContainerName");

        Document randomDocument = new Document
        {
            FileName = randomFileName,
            DocumentData = Encoding.ASCII.GetBytes(GetRandomString())
        };

        // When
        await this.documentService.RemoveDocumentByFileNameAsync(randomDocument.FileName);

        // Then
        this.blobStorageBrokerMock.Verify(broker =>
            broker.DeleteFileAsync(randomDocument.FileName, blobContainerName),
                Times.Once);

        this.blobStorageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}
