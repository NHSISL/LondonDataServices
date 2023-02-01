// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Documents;
public partial class DocumentServiceTests
{
    [Fact]
    public async Task ShouldDeleteFileAsync()
    {
        // Given
        string randomFileName = GetRandomString();
        var isDecrypted = false;

        Document randomDocument = new Document
        {
            FileName = randomFileName,
            DocumentData = Encoding.ASCII.GetBytes(GetRandomString())
        };

        // When
        await this.documentService.RemoveDocumentByFileNameAsync(randomDocument.FileName, isDecrypted);

        // Then
        this.blobStorageBrokerMock.Verify(broker =>
            broker.DeleteFileAsync(randomDocument.FileName, isDecrypted),
                Times.Once);

        this.blobStorageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}
